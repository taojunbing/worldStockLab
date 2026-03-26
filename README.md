# 🌍 WorldStockLab

一个基于 ASP.NET Core 构建的个人金融博客 + 股票数据平台（Demo版）

---

## 🎯 项目目标

本项目用于：

* 展示美股（纳斯达克100 + ETF）行情
* 提供股票分析博客功能
* 探索金融数据网站架构设计
* 作为前后端分离架构的过渡版本

> 当前版本为 MVC Demo，后续将升级为 Vue + .NET Web API 架构

---

## ✨ 功能列表

### 📊 股票模块

* 股票列表（纳指100）
* 股票详情页（集成 TradingView 图表）
* 热门股票（浏览量排序）
* 涨跌榜（Top Gainers / Losers）
* 首页市场概览（三大指数）

### 📰 博客模块

* 文章发布（Markdown 编辑器）
* 文章列表
* 股票关联文章（按 Symbol 绑定）

### ⚙️ 系统功能

* RSS 订阅
* Sitemap（SEO优化）
* 股票后台定时更新（BackgroundService）

### 🎨 UI

* 自定义金融风格界面
* 首页分类模块（AI / ETF / 半导体）
* 响应式布局（基础）

---

## 🏗 技术架构

### 后端

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server

### 前端

* Razor Pages
* Toast UI Editor（Markdown 编辑器）
* TradingView Widget（K线图）

### 数据来源

* Finnhub API（股票行情）
* TradingView（图表展示）

### 后台任务

* BackgroundService 定时更新股票数据

---

## 📁 项目结构

```
worldStockLab.Web
│
├── Controllers
│   ├── HomeController.cs
│   ├── StockController.cs
│   ├── BlogController.cs
│   └── SitemapController.cs
│
├── Models
│   ├── StockPrice.cs
│   └── Article.cs
│
├── Services
│   ├── FinnhubService.cs
│   └── StockPriceUpdater.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Views
│   ├── Home/
│   ├── Stock/
│   └── Blog/
│
└── wwwroot
    ├── css/
    └── js/
```

---

## 🗄 数据库设计

### StockPrices

* Symbol
* Price
* Change
* ChangePercent
* Volume
* UpdatedAt

### Articles

* Id
* Title
* Content
* Summary
* Category
* Symbol
* CreatedAt

---

## 🚀 本地运行

1. 克隆项目

```bash
git clone https://github.com/yourname/worldstocklab.git
```

2. 修改数据库连接（appsettings.json）

3. 执行数据库迁移

```bash
dotnet ef database update
```

4. 启动项目

```bash
dotnet run
```

5. 访问

```
http://localhost:5082
```

---
<img width="1662" height="1330" alt="屏幕截图 2026-03-26 140633" src="https://github.com/user-attachments/assets/79cc7935-73fb-4254-b5a4-3dd60bb4f8a5" />


## ⚠️ 已知问题（Known Issues）

### 📝 博客发布功能异常

当前版本中，后台文章发布功能存在问题：

* 使用 Toast UI Editor 时，Markdown 内容未能稳定提交到后端
* 表单提交后，Content 字段在部分情况下为空
* 发布操作偶尔失败或未写入数据库

#### 可能原因

* 编辑器内容未正确绑定到隐藏字段（Content）
* 表单提交时 JS 执行时机不稳定
* MVC Model Binding 未正确接收字段

#### 当前处理方式

* 页面中已使用 hidden input 存储 Markdown 内容
* 使用 `editor.getMarkdown()` 在提交前赋值
* 但在某些情况下仍然失效

#### 后续优化计划

该问题将在前后端分离版本中彻底解决：

* 前端使用 Vue 管理编辑器状态
* 通过 API 显式提交 Markdown 内容
* 避免 Razor + JS 混合带来的不确定性

> 该问题不影响股票模块及整体系统运行

---

### 📌 技术反思

本问题暴露了传统 MVC 模式在复杂前端交互（富文本编辑器）下的局限性，
也是本项目升级为前后端分离架构的重要原因之一。

---

## 🚀 Roadmap

### v2.0（前后端分离）

* Vue3 + Vite 前端
* ASP.NET Core Web API
* JWT 用户认证

### v2.1

* 股票搜索自动补全优化
* 用户系统（登录 / 收藏）

### v2.2

* 市场情绪指标（Fear & Greed）
* 行业分类系统

### v3.0

* AI 辅助生成股票分析
* 自动内容推荐

---

## 💡 项目价值

本项目体现了：

* 金融数据网站基础架构设计能力
* ASP.NET Core 全栈开发能力
* 后台任务调度（BackgroundService）
* SEO优化（Sitemap + RSS）
* 前端UI设计能力

适合作为：

✔ 求职项目
✔ 架构演进练习
✔ 个人技术博客平台

---

## 🧭 项目定位说明

该项目为学习型 Demo，不追求高精度行情系统，而是侧重：

👉 内容 + 数据展示 + 架构设计
