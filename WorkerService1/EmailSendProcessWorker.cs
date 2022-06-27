using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService1
{
    internal class EmailSendProcessWorker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _emailService;

        public EmailSendProcessWorker(ILogger<Worker> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Uygulama başlarken çalışır.
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //Uygulama kapanmadan önce yapılması gerekenler burada yazar.
            //En önemli kısımdır.
            //Alert kanalı eklenebilir.
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*Uygulama bir kere çalışacaksa while döngüsüne ihtiyacımız olmayacaktır.
             * Ancak event'i olmayan mesaj kuyruk sistemlerinde mutlaka "while" döngüsü kullanmamız lazım.*/

            while (!stoppingToken.IsCancellationRequested)
            {
                _emailService.send("Sea");
                _logger.LogInformation("EmailSendProcessWorker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}