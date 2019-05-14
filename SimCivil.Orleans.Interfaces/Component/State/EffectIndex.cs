// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Orleans.Interfaces - EffectIndex.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/13

using System;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Component.State
{
    public enum EffectIndex
    {
        /// <summary>
        /// 具现系魔法强度
        /// </summary>
        MaterialMagicPower,

        /// <summary>
        /// 变形系魔法强度
        /// </summary>
        DeformationMagicPower,

        /// <summary>
        /// 增强/衰减系魔法强度
        /// </summary>
        BuffMagicPower,

        /// <summary>
        /// 分解系魔法强度
        /// </summary>
        DecompositionMagicPower,

        /// <summary>
        /// 魔法学习效率
        /// </summary>
        MagicLearningEfficiency,

        /// <summary>
        /// 精神抗性
        /// </summary>
        SpiritResistance,

        /// <summary>
        /// 召唤系魔法强度
        /// </summary>
        SummonMagicPower,

        /// <summary>
        /// 时空系魔法强度
        /// </summary>
        SpaceTimeMagicPower,

        /// <summary>
        /// 感知系魔法强度
        /// </summary>
        PerceptionMagicPower,

        /// <summary>
        /// 魔力转换效率
        /// </summary>
        MagicTransformEfficiency,

        /// <summary>
        /// 控制系魔法强度
        /// </summary>
        ControllingMagicPower,

        /// <summary>
        /// 魔法命中
        /// </summary>
        MagicHitRate,

        /// <summary>
        /// 魔法学习速率
        /// </summary>
        MagicLearningRate,

        /// <summary>
        /// 瞄准精度
        /// </summary>
        AimingAccuracy,

        /// <summary>
        /// 视野距离
        /// </summary>
        SightRange,

        /// <summary>
        /// 发明基础概率
        /// </summary>
        InventionProbability,

        /// <summary>
        /// 瞄准速率
        /// </summary>
        AimingSpeed,

        /// <summary>
        /// 技能学习效率
        /// </summary>
        SkillLearningRate,

        /// <summary>
        /// 消化效率
        /// </summary>
        DigestionEfficiency,

        /// <summary>
        /// 最大体力
        /// </summary>
        MaximumEndurance,

        /// <summary>
        /// 适宜温度范围
        /// </summary>
        TemperatureRange,

        /// <summary>
        /// 负重
        /// </summary>
        MaximumLoad,

        /// <summary>
        /// 上肢攻击基础效率
        /// </summary>
        UpperAttackEfficiency,

        /// <summary>
        /// 上肢基础命中率
        /// </summary>
        UpperAttackHitRate,

        /// <summary>
        /// 上肢攻击基础速率
        /// </summary>
        UpperAttackSpeed,

        /// <summary>
        /// 操作效率
        /// </summary>
        OperationEfficiency,

        /// <summary>
        /// 制造效率
        /// </summary>
        CraftEfficiency,

        /// <summary>
        /// 基础闪避率
        /// </summary>
        DodgeRate,

        /// <summary>
        /// 最大跳跃高度
        /// </summary>
        MaximumJumpHeight,

        /// <summary>
        /// 下肢攻击基础效率
        /// </summary>
        LowerAttackEfficiency,

        /// <summary>
        /// 下肢基础命中率
        /// </summary>
        LowerAttackHitRate,

        /// <summary>
        /// 下肢攻击基础速率
        /// </summary>
        LowerAttackSpeed,

        /// <summary>
        /// 移动基础速率
        /// </summary>
        MoveSpeed,
        EffectCount
    }
}