# Что такое TraceId ![.NET Core](https://github.com/Calabonga/Calabonga.Microservices.Tracker/workflows/.NET%20Core/badge.svg)

В микросервисной архитектуре много сервисов по определению. Значит одна бизнес-операция может "пролегать" через несколько сервисов, а значит множество REST-запросов "полетят" по разным WebAPI. Например, оплата в интернет-магазине товара является бизнес-операцией, которая может состоять из нескольких запросов: оплата счета, отправка уведомлений, сбор статистики и т.д. И каждый из перечисленных запросов может (и должен) обрабатываться отдельным сервисом. Вопрос, как фильтровать все запросы для одной бизнес-операции чтобы отследить результаты? Ответ - надо чтобы все запросы были помечены одной и той же меткой (идентификатором). Такой идентификатор обычно называют CorrelationId (я называю TraceId, потому что так короче и писать проще).

## Как это работает

Работает система на базе заголовков (headers)  запросов. И в REST-запросах, и в MessageQueue-запросах...  Почему используются "заголовки"? Перефразирую вопрос, почему нельзя сделать ViewModel, к котором будет TraceId? Тезисно:

- Потому что эта операция стоит над бизнес-логикой и, следовательно, не должна смешиваться.
- Потому что ваши ViewModels будут меняться, появляться, исчезать, а запросы изменяются очень и очень редко.
- Потому что обработка TraceId происходит в том числе и другими сервисами. Например, для сбора статистики или мониторинга. А ваши ViewModels никто "залезать" не будет.

## Как реализовать в ASP.NET Core?
Уже готовых решений в интернете полным-полно. Я использую библиотеку Calabonga.Microservices.Tracker, которой решил поделиться совершенно недавно. Как это работает.

- Установливаете nuget-пакет
- Настраиваете пару параметров
- И вуаля!

[Инструкция по применению](https://www.calabonga.net/blog/post/tracking-correlationid-between-microservices-asp-net-core)

Library helps to generate a trace ID string for the cross-microservice communication.

## Испория версий

### v6.0.0 2024-06-21

Добавлены несколько расширений для возможности регистрации `TrackerMiddleware` с дополнительные параметрами.
``` csharp
diff --git a/src/Calabonga.Microservices.Tracker/Extensions/TrackerServiceCollectionExtensions.cs b/src/Calabonga.Microservices.Tracker/Extensions/TrackerServiceCollectionExtensions.cs
--- a/src/Calabonga.Microservices.Tracker/Extensions/TrackerServiceCollectionExtensions.cs
+++ b/src/Calabonga.Microservices.Tracker/Extensions/TrackerServiceCollectionExtensions.cs
@@ -45,0 +45,114 @@
+        /// <summary>
+        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
+        /// </summary>
+        /// /// <remarks>
+        /// This operation is idempotent - multiple invocations will still only result in a single
+        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
+        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
+        /// </remarks>
+        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
+        /// <param name="trackerOptions"></param>
+        public static ITrackerBuilder AddCommunicationTracker(this IServiceCollection services, Action<TrackerOptions> trackerOptions)
+        {
+            // removed for clarity
+        }
+
+        /// <summary>
+        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
+        /// </summary>
+        /// /// <remarks>
+        /// This operation is idempotent - multiple invocations will still only result in a single
+        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
+        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
+        /// </remarks>
+        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
+        /// <param name="excludeOptions"></param>
+        public static ITrackerBuilder AddCommunicationTracker(this IServiceCollection services, Action<ExcludeOptions> excludeOptions)
+        {
+            // removed for clarity
+        }
+
+        /// <summary>
+        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
+        /// </summary>
+        /// /// <remarks>
+        /// This operation is idempotent - multiple invocations will still only result in a single
+        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
+        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
+        /// </remarks>
+        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
+        /// <param name="trackerOptions"></param>
+        /// <param name="excludeOptions"></param>
+        public static ITrackerBuilder AddCommunicationTracker(this IServiceCollection services, Action<TrackerOptions> trackerOptions, Action<ExcludeOptions> excludeOptions)
+        {
+            // removed for clarity
+        }

```

### v5.0.0 2024-05-21

- Добавлена возможность использовать исключения при генерации TraceID по типам `Request.Scheme`,`Request.Host` и `Request.Path`. Например, когда Prometheus делает запросы на ваш сервер, чтобы забрать метркику `/metrics`, то в этом случае совершенно не трбуется генерация TrageId. Пример добавления исключений ниже:

    ``` csharp
                // services.AddCommunicationTracker();
            // services.AddCommunicationTracker<CustomTrackerIdGenerator>();
            services.AddCommunicationTracker<CustomTrackerIdGenerator>(
                options =>
                {
                    // The commented out values below is default
                    // options.TrackerIdGenerator = () => "qweqweqwewqeqwe";
                    // options.EnforceHeader = false;
                    // options.IgnoreRequestHeader = false;
                    // options.RequestHeaderName = "X-Custom-Request-Trace-ID";
                    // options.ResponseHeaderName = "X-Custom-Response-Trace-ID";
                    // options.AddToLogger = true;
                    // options.LoggerScopeName = "MICROSERVICE_LOGGER";
                    // options.IncludeInResponse = true;
                    options.UpdateTraceIdentifier = true;
                },
                excludes =>
                {
                    excludes
                        .AddPathExcludes("activities", CheckExcludeType.Contains)  // <-- for path
                        .AddPathExcludes("api", CheckExcludeType.Contains)         // <-- for path
                        .AddSchemeExcludes("https", CheckExcludeType.Equality)     // <-- for schemes
                        .AddHostExcludes("localhost", CheckExcludeType.StartWith); // <-- for hosts
                });
    ```

- Обновлены зависимости
- Все логи типа "Information" переведены в разряд `Trace`, чтобы уменьшить количество логов на `Production`.