# Coordinates
Был реализован Web API для работы с координатами. Решение содрежит 2 проекта - Web API сервис и NUnit тесты. 
Что было в новинку:
Использование .Net 8 и C# 12 и изучение их особенностей;
Тестирование с фреймфорком NUnit, а также интеграционное тестирование с использованием WebApplicationFactory и HttpClient;
Использование класса JsonSerializer для сериализации/десериализации json;
Трудности:
Возвращение ответа с милями и метрами, значения в которых равны нулю, тогда когда body в post контоллере - null;
Параметризированный тест с входнными параметрами сылочного типа;
