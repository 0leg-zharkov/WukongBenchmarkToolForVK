using System.Diagnostics;
using WukongBenchmarkToolForVK.Automation;

namespace WukongBenchmarkToolForVK;

public class BenchmarkRunner
{
    // время на первую загрузку
    private const int UiLoadDelayMs = 35000;

    public async Task AutoStartTestAsync(Process game)
    {
        Console.WriteLine("[Runner] Ждём прогрузки UI (35 сек)...");
        await Task.Delay(UiLoadDelayMs);

        WindowController.HideConsole();
        await Task.Delay(2000);

        // нажали по приветственному меню
        WindowController.ClickCenter(game);
        await Task.Delay(1500);

        Console.WriteLine("[Runner] -> Space (приветствие)");
        KeyboardController.SendScancode(KeyboardController.VK_SPACE);
        await Task.Delay(4000);

        Console.WriteLine("[Runner] -> Enter (страховка)");
        KeyboardController.SendScancode(KeyboardController.VK_ENTER);
        // успеть загрузить главное меню
        await Task.Delay(16000);

        // нажатие на запуск теста
        Console.WriteLine("[Runner] Клик по 'Тест быстродействия'...");
        WindowController.ClickAt(game, 0.115, 0.444);
        await Task.Delay(4000);

        // подтвердить нажатие
        Console.WriteLine("[Runner] Клик по центру (диалог подтверждения)...");
        WindowController.ClickCenter(game);
        await Task.Delay(1500);

        Console.WriteLine("[Runner] -> Enter (подтвердить)");
        KeyboardController.SendScancode(KeyboardController.VK_ENTER);
    }

    public async Task FinishTestAndExitMenuAsync(Process game)
    {
        Console.WriteLine("[Runner] Выходим из меню результатов...");

        WindowController.ClickCenter(game);
        await Task.Delay(1500);

        Console.WriteLine("[Runner] -> Esc");
        KeyboardController.SendScancode(KeyboardController.VK_ESC);
        await Task.Delay(2500);

        WindowController.ClickCenter(game);
        await Task.Delay(1000);

        Console.WriteLine("[Runner] -> Enter (подтвердить выход)");
        KeyboardController.SendScancode(KeyboardController.VK_ENTER);
        await Task.Delay(3000);
    }
}