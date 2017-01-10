using Hellion.Core.IO;
using System;
using System.Linq;

namespace Hellion.World.Systems
{
    public class Chat
    {
        private Player player;

        /// <summary>
        /// Creates a new instance of the Chat module.
        /// </summary>
        /// <param name="owner"></param>
        public Chat(Player owner)
        {
            this.player = owner;
        }

        /// <summary>
        /// Executes a regular or GM command.
        /// </summary>
        /// <param name="chatMessage"></param>
        public void CommandChat(string chatMessage)
        {
            string[] chatCommandArray = chatMessage.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (chatCommandArray.Length > 1)
            {
                var chatCommand = chatCommandArray.First();

                if (this.IsNormalCommand(chatCommand))
                    this.NormalCommandChat(chatCommand, chatMessage);
                else if (this.IsGMCommand(chatCommand, chatCommandArray) && this.player.Authority >= 80)
                    this.GMCommand(chatCommand, chatCommandArray);
                else if (this.IsADMINCommand(chatCommand, chatCommandArray) && this.player.Authority >= 100)
                    this.GMCommand(chatCommand, chatCommandArray);
            }
        }

        /// <summary>
        /// Executes a regular command.
        /// </summary>
        /// <param name="chatMessageCommand"></param>
        /// <param name="chatMessage"></param>
        private void NormalCommandChat(string chatMessageCommand, string chatMessage)
        {
        }

        /// <summary>
        /// Executes a GM command.
        /// </summary>
        /// <param name="chatCommand"></param>
        /// <param name="chatCommandArray"></param>
        private void GMCommand(string chatCommand, string[] chatCommandArray)
        {
            switch (chatCommand)
            {
                default:
                    Log.Info("Unknow GM command '{0}'.", chatCommand);
                    break;
            }
        }

        /// <summary>
        /// Check if the chat command is a regular GM command..
        /// </summary>
        /// <param name="chatCommand"></param>
        /// <param name="chatCommandArray"></param>
        private bool IsGMCommand(string chatCommand, string[] chatCommandArray)
        {
            switch (chatCommand)
            {
                case "/teleport":
                case "/invisible":
                case "/noinvisible":
                case "/summon":
                case "/count":
                case "/out":
                case "/talk":
                case "/notalk":
                case "/ip":
                case "/GuildRanking":
                case "/FallSnow":
                case "/StopSnow":
                case "/FallRain":
                case "/StopRain":
                case "/system":
                case "/onekill":
                case "/noonekill":
                case "/undying":
                case "/undying2":
                case "/noundying":
                case "/aroundkill":
                case "/stat":
                case "/level":
                case "/IniSkillExp":
                case "/skilllevel":
                case "/skilllevelAll":
                case "/freeze":
                case "/nofreeze":
                case "/PartyLevel":
                case "/BeginQuest":
                case "/EndQuest":
                case "/RemoveQuest":
                case "/RemoveAllQuest":
                case "/RemoveCompleQuest":
                case "/changejob":
                case "/GuildStat":
                case "/createguild":
                case "/destroyguild":
                case "/GCIn":
                case "/GCOpen":
                case "/GCClose":
                case "/GCNext":
                case "/indirect":
                case "/createnpc":
                case "/EVENTLIST":
                case "/EVENTINFO":
                case "/gamesetting":
                case "/rmvnpc":
                case "/PvpParam":
                case "/PKParam":
                    return true;
                default: return false;
            }
        }

        /// <summary>
        /// Check if the chat command is a ADMIN command.
        /// </summary>
        /// <param name="chatCommand"></param>
        /// <param name="chatCommandArray"></param>
        /// <returns></returns>
        private bool IsADMINCommand(string chatCommand, string[] chatCommandArray)
        {
            switch (chatCommand)
            {
                case "/disguise":
                case "/nodisguise":
                case "/ResistItem":
                case "/jobname":
                case "/getgold":
                case "/createitem":
                case "/createitem2":
                case "/QuestState":
                case "/loadscript":
                case "/ReloadConstant":
                case "/ctd":
                case "/Piercing":
                case "/petlevel":
                case "/petexp":
                case "/makepetfeed":
                case "/Pet":
                case "/Lua":
                case "/GC1TO1OPEN":
                case "/GC1TO1CLOSE":
                case "/GC1TO1NEXT":
                case "/RefineAccessory":
                case "/RefineCollector":
                case "/GenRandomOption":
                case "/InitializeRandomOption":
                case "/SetRandomOption":
                case "/SetPetName":
                case "/ClearPetName":
                case "/Propose":
                case "/Refuse":
                case "/Couple":
                case "/Decouple":
                case "/ClearPropose":
                case "/CoupleState":
                case "/NextCoupleLevel":
                case "/RemoveBuff":
                case "/HonorTitleSet":
                case "/open":
                case "/close":
                case "/music":
                case "/sound":
                case "/localevent":
                case "/CommercialElem":
                case "/SetPlayerName":
                case "/SetGuildName":
                case "/DeclWar":
                case "/rgm":
                case "/GuildRankingUpdate":
                case "/gmitem":
                case "/gmnoitem":
                case "/gmattck":
                case "/gmnoattck":
                case "/gmcommunity":
                case "/gmnotcommunity":
                case "/gmobserve":
                case "/gmonotobserve":
                case "/EscapeReset":
                case "/userlist":
                case "/sbready":
                case "/sbstart":
                case "/sbstart2":
                case "/sbend":
                case "/sbreport":
                case "/bsopen":
                case "/bsclose":
                case "/SetGuildQuest":
                case "/Snoop":
                case "/SnoopGuild":
                case "/GCRequest":
                case "/GCCancel":
                case "/PostMail":
                case "/RemoveMail":
                case "/GetMailItem":
                case "/GetMailGolg":
                case "/InvenClear":
                case "/ExpUpStop":
                case "/createchar":
                case "/createctrl":
                case "/setmonsterrespawn":
                case "/TransyItemList":
                case "/LoadToolTip":
                case "/monstersetting":
                case "/changeshopcost":
                case "/CallTheRoll":
                case "/AExp":
                case "/RemAttr":
                case "/StartCollecting":
                case "/StopCollecting":
                case "/Battery":
                case "/AvailPocket":
                case "/PocketView":
                case "/MoveItemPocket":
                case "/ItemLevel":
                case "/COUPON":
                case "/CreatePc":
                case "/SfxLevel":
                case "/SROPEN":
                case "/SRNEXT":
                case "/SRENTRANCE":
                case "/SRTENDER":
                case "/SRLINEUP":
                case "/SRCLOSE":
                case "/SRVIEW":
                case "/SRCANCEL":
                case "/ElectionRequirement":
                case "/ElectionAddDeposit":
                case "/ElectionSetPledge":
                case "/ElectionIncVote":
                case "/ElectionProcess":
                case "/ElectionBeginCandidacy":
                case "/ElectionBeginVote":
                case "/ElectionEndVote":
                case "/ElectionState":
                case "/LEventCreate":
                case "/LEventInitialize":
                case "/LSkill":
                case "/RemoveTotalGold":
                case "/SetTutorialState":
                case "/TaxApplyNow":
                case "/HeavenTower":
                case "/RemoveJewel":
                case "/TransEggs":
                case "/PickupPetAwakeningCancel":
                case "/CreateLayer":
                case "/DeleteLayer":
                case "/Layer":
                case "/RRapp":
                case "/RROpen":
                case "/RRNext":
                case "/RRPass":
                case "/RRInfo":
                case "/RRRule":
                case "/RRRanking":
                case "/RRPrize":
                case "/RRKawiBawiBo":
                case "/RRDice":
                case "/RRArithmetic":
                case "/RRStopWatch":
                case "/RRTyping":
                case "/RRCard":
                case "/RRLadder":
                case "/RRFINISH":
                case "/CHATTRIBUTE":
                case "/HousingVisit":
                case "/HousingGMRemoveAll":
                case "/SmeltSafetyNormal":
                case "/SmeltSafetyAccessary":
                case "/SmeltSafetyPiercing":
                case "/SmeltSafetyElement":
                case "/QuizEventOpen":
                case "/QuizEventEnterance":
                case "/QuizStateNext":
                case "/QuizEventClose":
                case "/BuyGuildHouse":
                case "/GuildHouseUpKeep":
                case "/RemoveCampusMember":
                case "/UpdateCampusPoint":
                case "/InvenRemove":
                    return true;
                default: return false;
            }
        }

        /// <summary>
        /// Check if the chat command is a regular command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool IsNormalCommand(string command)
        {
            switch (command)
            {
                case "/s":
                case "/shout":
                case "/w":
                case "/whisper":
                case "/p":
                case "/party":
                case "/g":
                case "/guild":
                case "/partyinvite":
                case "/guildinvite":
                    return true;
                default: return false;
            }
            
        }
    }
}
