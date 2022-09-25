# How do I get started?
#### Install
    Install-Package TieuAnhQuoc.HandleLog

#### Add setting
    builder.Services.AddLogging(logging =>
    {
        logging.AddProvider(new FileLoggerProvider("your_log_folder"));
    });