SimCivil
=======================
![](https://img.shields.io/github/release/tcz717/SimCivil.svg)
![](https://img.shields.io/badge/.net-4.6.1%7Ccore%202.0-blue.svg)
[![Build Status](https://travis-ci.com/tcz717/SimCivil.svg?branch=master)](https://travis-ci.com/tcz717/SimCivil)
[![](https://img.shields.io/appveyor/tests/tcz717/SimCivil.svg?compact_message)](https://ci.appveyor.com/project/tcz717/simcivil)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/8378da8892a947d1be1bafa316bd378e)](https://www.codacy.com/app/tcz717/SimCivil?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=tcz717/SimCivil&amp;utm_campaign=Badge_Grade)
![](https://img.shields.io/github/downloads/tcz717/SimCivil/total.svg)
![](https://img.shields.io/github/license/tcz717/SimCivil.svg)
[![Sentery](https://img.shields.io/badge/sentery-enabled-blue.svg)](https://sentry.io/tpdt/simcivil/)
<a href="https://zenhub.com"><img src="https://raw.githubusercontent.com/ZenHubIO/support/master/zenhub-badge.png"></a>

此项目主要是游戏的服务端，负责核心逻辑处理

A C# game server simulating a civilization world allowing roles free interacting. The key goals of the game are to dynamically generate any skills, technologies, recipes, and objects according to role behavior, and imitating real-world roles interacting logic as much as possible.

## 介绍
服务器模拟一个非常自由或者说内容设定丰富的世界，世界里面的 **所有人形生物** 都是玩家写脚本自动控制的。游戏不提供官方的客户端，玩家可以选择使用人工智能、有限状态机、甚至手动控制的方式，利用服务器提供的API控制玩家的角色在游戏内生存、发展和竞争。

## 服务器启动方式
1. 编译并运行`SimCivil.Orleans.Server`
1. 编译并运行`SimCivil.Gate`

## 客户端
目前有一个@jykgod 基于Cocos Creator的2d客户端实现[Ezgame](https://github.com/jykgod/Ezgame) 

## 讨论

- 如果对于项目设计有任何建议或者反馈，可以提交issue一起讨论
- 错误日志汇总可以在[sentry](https://sentry.io/tpdt/simcivil/)上查看

## 配置开发环境
- [参考wiki](https://github.com/tcz717/SimCivil/wiki/%E5%BC%80%E5%8F%91%E7%8E%AF%E5%A2%83%E9%85%8D%E7%BD%AE)

## 主要模块

- 游戏核心逻辑
- 服务器网络通讯
- 数据持久化（存档读档）
- 图形化工具，包括数据编辑器，简易地图查看器
- 其他辅助算法如A*寻路算法、Perlin地图生成算法等

## 主要概念
- [参考Wiki](https://github.com/tcz717/SimCivil/wiki)

## 项目依赖

- [.net core 2.0](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0.0.md)
- [Newtonsoft.Json 10.0.3](https://www.newtonsoft.com/json)
- [Autofac 4.6.1](http://docs.autofac.org/en/latest/index.html)
- [Log4net](http://logging.apache.org/log4net/)
- System.ValueType 4.4.0
