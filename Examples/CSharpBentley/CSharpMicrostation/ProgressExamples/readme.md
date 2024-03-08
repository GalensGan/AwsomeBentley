# 进度条相关开发文档

> 本测试模块依赖于 [GalensGan/MSAddinTest: A manager for Bentley Microstation Addin test (github.com)](https://github.com/GalensGan/MSAddinTest) 组件

## 功能特点

- 不会阻塞
- 异常自动关闭

## MSProgress

执行命令 `mstest test TestMSProgress` 可查看效果。

使用示例：

``` csharp
public static void TestMSProgress(string unparsed)
{
    var progress = new MSProgress("test");
    progress.Run(() =>
    {
        for (int i = 0; i < 100; i++)
        {
            var message = $"当前进度: {i}";
            progress.Update(message, i + 1);
            Thread.Sleep(50);
        }
    });
}
```

## ThreadProgress

执行命令 `mstest test TestThreadProgress` 可查看效果。

使用示例：

``` csharp
[MSTest("TestThreadProgress")]
public static void TestThreadProgress(string unparsed)
{
    var startDate = DateTime.Now;
    var progress = new ThreadProgress("test");
    progress.Run(()=>
    {
        var sameNameProgress = new ThreadProgress("test");
        for (int i = 0; i < 100; i++)
        {
            var message = $"当前进度: {i}";
            Logger.Log(message);
            if (i % 2 == 0) progress.Update(message, i + 1);
            else sameNameProgress.Update(message, i + 1);

            Thread.Sleep(50);
        }
    });

    var endDate = DateTime.Now;
    Logger.Log($"耗时：{endDate - startDate}", "");
}
```

