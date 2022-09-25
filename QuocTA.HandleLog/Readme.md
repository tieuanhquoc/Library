# How do I get started?
#### Install
    Install-Package QuocTa.HandleLog

#### Add setting
    builder.Services.AddLogging(logging =>
    {
        logging.AddProvider(new FileLoggerProvider("your_log_folder"));
    });