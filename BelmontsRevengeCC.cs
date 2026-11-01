using System;
using System.Collections.Generic;
using System.Threading;
using CrowdControl.Common;
using CrowdControl.Games;
using ConnectorLib;
using ConnectorType = CrowdControl.Common.ConnectorType;

namespace CrowdControl.Games.Packs.BelmontsRevengeCC;

public class BelmontsRevengeCC : GBEffectPack
{
    public BelmontsRevengeCC(UserRecord player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler)
        : base(player, responseHandler, statusUpdateHandler) { }
    public override Game Game { get; } = new("Belmont's Revenge - CC alpha1", "BelmontsRevengeCC", "GB", ConnectorType.GBConnector);
    public override ROMTable ROMTable => new[]
    {
        new ROMInfo("Belmont CC alpha1 (prepatched)", null, Patching.Ignore, ROMStatus.ValidPatched,
            s => Patching.MD5(s, "a0beaa14075fdabe3b78bd96ea0c7912"))
    };
    public override EffectList Effects => new Effect[]
    {
        new("Give Axe", "weapon_axe") { Price=50, Category="Equipment" },
        new("Give Holy Water", "weapon_water") { Price=50, Category="Equipment" },
        new("Give Cross", "weapon_cross") { Price=50, Category="Equipment" },
        new("Remove Subweapon", "weapon_none") { Price=50, Category="Equipment" },
        new("99 Hearts", "hearts_max") { Price=50, Category="Player" },
        new("Remove Hearts", "hearts_none") { Price=50, Category="Player" },
        new("Full Health", "health_full") { Price=100, Category="Player" },
        new("Kill Christopher", "kill_player") { Price=200, Category="Player" }
    };
    private readonly object _commandLock = new();
    private static readonly Dictionary<string,byte> Commands = new()
    {
        ["weapon_axe"]=1,["weapon_water"]=2,["weapon_cross"]=3,["weapon_none"]=4,
        ["hearts_max"]=5,["hearts_none"]=6,["health_full"]=7,["kill_player"]=8
    };
    private const uint Mailbox=0xC8E0;
    private bool Read(uint a,out byte b) => Connector.Read8(a,out b);
    private bool Signature() => Read(Mailbox,out byte a)&&a==66 && Read(Mailbox+1,out a)&&a==82
        && Read(Mailbox+2,out a)&&a==67 && Read(Mailbox+3,out a)&&a==67 && Read(Mailbox+4,out a)&&a==1;
    protected override GameState GetGameState()
    {
        if (!Signature()) return GameState.WrongMode;
        if (!Read(0xC880,out byte mode)||mode!=5 || !Read(0xC881,out byte sub)||sub!=0
            || !Read(0xC88B,out byte paused)||(paused&3)!=0 || !Read(0xCAE0,out byte transition)||transition!=0
            || !Read(Mailbox+9,out byte ready)||(ready&1)==0) return GameState.WrongMode;
        return GameState.Ready;
    }
    protected override void StartEffect(EffectRequest request)
    {
        if (!Commands.TryGetValue(FinalCode(request),out byte command))
        { Respond(request,EffectStatus.FailPermanent,StandardErrors.EffectUnknown,request); return; }
        lock (_commandLock)
        {
            if (GetGameState()!=GameState.Ready || !Read(Mailbox+9,out byte ready)
                || (command<=4 && (ready&2)==0)) { Respond(request,EffectStatus.FailTemporary); return; }
            if (!Read(Mailbox+5,out byte pending)||!Read(Mailbox+6,out byte ack)||pending!=ack)
            { Respond(request,EffectStatus.FailTemporary); return; }
            uint[] context={0xC8C0,0xC8C1,0xCA86,0xCA8D};
            for(uint i=0;i<4;i++)
                if(!Read(context[i],out byte value)||!Connector.Write8(Mailbox+12+i,value))
                { Respond(request,EffectStatus.FailTemporary); return; }
            byte seq=unchecked((byte)(ack+1));
            if(!Connector.Write8(Mailbox+7,command)||!Connector.Write8(Mailbox+8,0))
            { Respond(request,EffectStatus.FailTemporary); return; }
            // Sequence write is the commit. After this point uncertainty must
            // not automatically retry Kill on a subsequent life.
            if(!Connector.Write8(Mailbox+5,seq)) { Respond(request,EffectStatus.FailPermanent); return; }
            long deadline=Environment.TickCount64+2000;
            while(Environment.TickCount64<deadline)
            {
                if(Read(Mailbox+6,out byte observed)&&observed==seq && Read(Mailbox+8,out byte result))
                {
                    Respond(request,result==1?EffectStatus.Success:result==2?EffectStatus.FailTemporary:EffectStatus.FailPermanent);
                    return;
                }
                Thread.Sleep(10);
            }
            // Cancel an unacknowledged request so it cannot execute on resume.
            // Timeout is permanent/uncertain, never success or an automatic retry.
            Connector.Write8(Mailbox+6,seq);
            Respond(request,EffectStatus.FailPermanent);
        }
    }
}
