# Zabbix Monitor Viewer

Windows-приложение на C# / .NET 8 для безопасного и стабильного просмотра Zabbix через Microsoft Edge WebView2.

## Возможности

- Стартовое окно с URL, профилями и параметрами запуска.
- Сохранение настроек между перезапусками:
  - последний URL;
  - список профилей;
  - автозапуск с последним URL;
  - fullscreen по умолчанию;
  - автообновление и интервал;
  - запуск свернутым в трей;
  - запуск с Windows.
- Автообновление страницы с защитой от refresh во время загрузки.
- Ручное обновление (кнопка, `F5`, `Ctrl+R`) и отображение времени последнего обновления.
- Управление fullscreen (`F11`, `Esc`).
- Трей-режим с контекстным меню: открыть, обновить, настройки, выйти.
- Усиленная политика безопасности навигации:
  - whitelist по доменам профилей;
  - блок новых окон/попапов;
  - блок скачивания файлов;
  - логирование заблокированных действий.
- Понятная обработка ошибок (URL, WebView2 runtime, ошибки загрузки и сертификатов, сбои WebView2).
- Файловое логирование ключевых событий и ошибок.

## Требования

- Windows 10/11
- .NET 8 SDK (для сборки)
- Microsoft Edge WebView2 Runtime

WebView2 Runtime: [официальная страница загрузки](https://developer.microsoft.com/microsoft-edge/webview2/)

## Запуск в разработке

```bash
dotnet restore
dotnet run
```

## Сборка

```bash
dotnet build -c Release
```

## Публикация

### Single-file self-contained (рекомендуется)

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

### Single-file framework-dependent

```bash
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

Результат: `bin/Release/net8.0-windows/win-x64/publish/ZabbixMonitor.exe`

## Где хранятся данные приложения

- Настройки: `%LocalAppData%/ZabbixMonitor/settings.json`
- Логи: `%LocalAppData%/ZabbixMonitor/logs/`

## Горячие клавиши

- `F5` / `Ctrl+R` - обновить страницу
- `F11` - включить/выключить fullscreen
- `Esc` - выйти из fullscreen

