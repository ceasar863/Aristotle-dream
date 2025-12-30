using UnityEngine;

public enum Operation_Mapping_Enum
{
    #region 玩家的移动操作
    Movement,
    Jump,
    Run,
    #endregion

    #region 武器相关操作

    #region 发射器
    Shoot,
    Aim,
    Grab,
    Special_Skill, //一些武器的特有战技?
    #endregion

    #endregion

    #region 消耗品的使用
    Use_Item,
    #endregion
}

public enum Map_Type_Enum
{
    Performed,
    Cancel,
    Started,
}