# LogSystem For Unity / Unity日志系统

## Install / 安装
* **UPM方式**: 在Unity Package Manager中添加 `https://github.com/Mymirral/MLogger.git?path=MirralLogger`
* **手动安装**: 下载MirralLogger文件夹并放入Assets目录

## Features / 功能特性

### Low GC Design / 低GC设计
- 采用字典缓存样式查找，避免运行时字符串分配
- 静态初始化确保最小化运行时开销
- 位运算实现高效日志过滤，减少条件判断开销

### Multiple Output Sinks / 多输出渠道
- **Unity Console Sink**: 原生控制台输出（MirralLogger/Runtime/Sink/UnityConsoleSink.cs）
- **Canvas UI Sink**: 游戏内Debug面板输出（MirralLogger/Runtime/Sink/CanvasTextSink.cs）
- **File Sink**: 日志文件持久化存储（MirralLogger/Runtime/Sink/LogFileSink.cs）
- 通过`MLogger.AddSink()`动态注册/移除任意数量的接收器

### Style Customization / 样式定制
- 通过MLoggerSetting资源在Unity编辑器中配置颜色方案
- 支持按日志级别(LogLevel)和分类(LogCategory)分别设置样式
- 实时预览功能，无需重新启动游戏

### Log Filtering / 日志过滤
- 位掩码过滤系统：`setting.LogFilter(level)`和`setting.LogFilter(category)`
- 支持组合过滤条件（如同时显示Warning和Error）
- 可在运行时动态调整过滤级别

## Configuration / 配置说明

### MLoggerSetting资源配置
在Unity编辑器中通过`Assets > Create > MirralDevTool > MLoggerSetting`创建配置文件

| 配置项 | 说明            | 示例                          |
|--------|---------------|-----------------------------|
| **Output Level** | 需要显示的输出级别     | 只输出选择级别                     |
| **Output Category** | 需要显示的日志分类     | 只输出选择类别                     |
| **Level Style** | 每个日志级别的颜色配置   | Error=红色, Info=蓝色           |
| **Category Style** | 每个日志分类的颜色配置   | Network=绿色, UI=黄色           |
| **Show Type** | 输出 级别样式/类别样式  | Level显示等级颜色, Category显示分类颜色 |
| **Debug Canvas Prefab** | 游戏内Debug面板预制体 | MDebugCanvas.prefab         |

## Usage Examples / 使用示例

### 基础日志输出
```csharp
// 简单日志
MLogger.Log("Message Log")
MLogger.Log("User logged in successfully", LogLevel.Info, LogCategory.Auth);

// 带上下文对象的日志（用于Unity控制台高亮）
MLogger.Log("Texture loaded", LogLevel.Debug, LogCategory.Resource, textureObject);
```

### 动态配置过滤器
```csharp
// 仅显示错误和网络日志
var setting = Resources.Load<MLoggerSetting>("MLoggerSetting");
setting.LogLevel = LogLevel.Error;
setting.LogCategory = LogCategory.Network;
```

### 注册自定义接收器
```csharp
// 创建并注册Canvas输出
var canvasSink = new CanvasTextSink();
canvasSink.Open();

MLogger.AddSink(canvasSink); //或在Open函数中添加

// 移除控制台输出（仅保留Canvas）
MLogger.RemoveSink(UnityConsoleSink.Instance);
```

## Extension Guide / 扩展指南

### 实现自定义Sink
1. 创建类实现`ILogSink`接口（MirralLogger/Runtime/Sink/ILogSink.cs）
2. 实现必要方法：
```csharp
public class CustomSink : ILogSink
{
    public void Open() { /* 初始化资源 */ }
    public void Close() { /* 释放资源 */ }
    public void Emit(LogEntry log) { /* 处理日志 */ }
}
```
3. 注册到MLogger：
```csharp
MLogger.AddSink(new CustomSink());
```

### 推荐扩展场景
- **远程日志**：实现网络传输Sink
- **崩溃报告**：实现异常捕获Sink
- **性能监控**：实现帧率/内存监控Sink

## Compatibility / 兼容性
- **Unity版本**: 2021.3+ (LTS)
- **平台支持**: 所有Unity支持的平台（Windows, Mac, iOS, Android等）
- **构建类型**: 仅在Development Build和Editor中生效

## Sample Scene / 示例场景
包含完整示例场景：`MirralLogger/Sample/Sample.unity`
演示了基础用法、样式配置
