using Microsoft.AspNetCore.SignalR;
using SignalR;

namespace BasicWorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IHubContext<StockHub, IStockService> _stockHub;
		private const string stockName = "Basic Stock Name";

		public Worker(ILogger<Worker> logger, IHubContext<StockHub, IStockService> stockHub)
		{
			_logger = logger;
			_stockHub = stockHub;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				Random random = new Random();
				decimal stockPrice = random.Next(1, 100);
				await _stockHub.Clients.All.SendStockPrice(stockName, stockPrice);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}