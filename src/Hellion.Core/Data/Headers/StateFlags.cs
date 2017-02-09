using System;

namespace Hellion.Core.Data.Headers
{
    [Flags]
    public enum StateFlags
    {
        OBJSTAF_COMBAT = 0x00000001,
        OBJSTAF_WALK = 0x00000002,
        OBJSTAF_SIT = 0x00000004,
        OBJSTAF_FLY = 0x00000008,
        OBJSTAF_ACC = 0x00000010,
        OBJSTAF_ETC = 0x00000020,
        OBJSTAF_ACCTURN = 0x00000040,
        OBJSTAF_TURBO = 0x00000080,
    }


    public enum OBJMSG
    {
        OBJMSG_NONE,
        OBJMSG_FORWARD,		// ÀüÁø ÇØ¶ó
        OBJMSG_BACKWARD,	// ÈÄÁø ÇØ¶ó
        OBJMSG_STAND,		// Á¦ÀÚ¸®¿¡ ¼­ÀÖ¾î¶ó
        OBJMSG_SITDOWN,		// ¾É¾Æ¶ó
        OBJMSG_STANDUP,		// ¼­¶ó.(¾É¾ÆÀÖÀ»¶§)
        OBJMSG_STOP,		// ¸ØÃç¶ó
        OBJMSG_ASTOP,		// ¸ØÃç¶ó
        OBJMSG_LTURN,		// ¿ÞÂÊÀ¸·Î µ¹¾Æ¶ó
        OBJMSG_RTURN,		// ¿À¸¥ÂÊÀ¸·Î µ¹¾Æ¶ó
        OBJMSG_STOP_TURN,		// µµ´Â°É ¸ØÃç¶ó
        OBJMSG_PICKUP,		// ÁÝ´Ù.
        OBJMSG_RESURRECTION,	// ºÎÈ°.
        OBJMSG_COLLECT,		// Ã¤Áý.
        OBJMSG_APPEAR,		// µîÀå
        OBJMSG_APPEAR2,		// µîÀå2
        OBJMSG_STUN,		// ½ºÅÏ»óÅÂ
        OBJMSG_JUMP,		// Á¡ÇÁ
        OBJMSG_MODE_COMBAT,		// ÀüÅõ¸ðµå°¡ µÇ¾î¶ó
        OBJMSG_MODE_PEACE,		// ÆòÈ­¸ðµå°¡ µÇ¾î¶ó,
        OBJMSG_MODE_WALK,		// °È±â¸ðµå°¡ µÇ¾î¶ó
        OBJMSG_MODE_RUN,		// ¶Ù±â¸ðµå°¡ µÇ¾î¶ó
        OBJMSG_MODE_FLY,		// ºñÇà¸ðµå°¡ µÇ¾î¶ó
        OBJMSG_MODE_GROUND,		// Áö»ó¸ðµå°¡ µÇ¾î¶ó
        OBJMSG_MODE_TURBO_ON,	// ¼ø°£ °¡¼Ó ¸ðµå.
        OBJMSG_MODE_TURBO_OFF,	// °¡¼Ó¸ðµå ÇØÁ¦.
        OBJMSG_MODE_TURN,
        OBJMSG_ACC_START,	// °¡¼Ó ½ÃÀÛ
        OBJMSG_ACC_STOP,	// °¡¼Ó ÁßÁö
        OBJMSG_ATK1,		// ¿¬Å¸°ø°Ý 1½ÃÀÛ
        OBJMSG_ATK2,		// ¿¬Å¸°ø°Ý 2½ÃÀÛ..
        OBJMSG_ATK3,
        OBJMSG_ATK4,
        OBJMSG_SP_ATK1,		// Æ¯¼ö°ø°Ý : ¸ó½ºÅÍ°¡ »ç¿ë.
        OBJMSG_SP_ATK2,
        OBJMSG_ATK_RANGE1,	// ¿ø°Å¸®°ø°Ý 1½ÃÀÛ - È°°°Àº...
        OBJMSG_ATK_MAGIC1,	// ¿Ïµå°ø°Ý
        OBJMSG_MELEESKILL,
        OBJMSG_MAGICSKILL,
        OBJMSG_MAGICCASTING,
        OBJMSG_DIE,		// Á×¾î¶ó!
        OBJMSG_DAMAGE,			// µ¥¹ÌÁö¸ÔÀ½.
        OBJMSG_DAMAGE_FORCE,	// °­µ¥¹ÌÁö - ¸ÂÀ¸¸é ³¯¾Æ°¨.
        OBJMSG_LOOKUP,
        OBJMSG_LOOKDOWN,
        OBJMSG_STOP_LOOK,
        OBJMSG_MOTION,
        OBJMSG_TURNMOVE,
        OBJMSG_TURNMOVE2,
        OBJMSG_BASE_RADY,		// ±âº» ½ÃÀü µ¿ÀÛ
        OBJMSG_TEMP,
        OBJMSG_TEMP2,	// µð¹ö±ë¿ë °ø°Ý¸Þ½ÃÁö
        OBJMSG_TEMP3,
        OBJMSG_DESTPOS,
        OBJMSG_DESTOBJ,
        OBJMSG_FALL,
        OBJMSG_LFORWARD,
        OBJMSG_RFORWARD,
        OBJMSG_STOP_RUN,
    }
}
