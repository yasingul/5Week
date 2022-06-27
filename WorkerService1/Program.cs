using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddHostedService<Worker>();
        services.AddHostedService<EmailSendProcessWorker>();
    })
    .Build();

await host.RunAsync();