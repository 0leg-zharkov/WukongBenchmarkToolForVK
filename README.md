# Wukong Benchmark Tool for VK

Автоматизированный инструмент для запуска Black Myth: Wukong Benchmark Tool на C#.

## Что делает

- Применяет настройки CPU-теста (1280×720, Low)
- Запускает бенчмарк, проходит меню, ждёт завершения
- Применяет настройки GPU-теста (1920×1080, Max)
- Запускает бенчмарк повторно
- Парсит JSON-результаты и выводит отчёт

## Требования

- Windows 10/11
- .NET 8.0
- Установленный Black Myth: Wukong Benchmark Tool (Steam)

## Запуск с указанием пути

```powershell

 .\WukongBenchmarkToolForVK.exe "D:\SteamLibrary\steamapps\common\Black Myth Wukong Benchmark Tool"
```
## Запуск без указания пути

```powershell

.\WukongBenchmarkToolForVK.exe
