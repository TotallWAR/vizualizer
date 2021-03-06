# vizualizer
Визуализатор - компонента распределенной системы доставки товаров различными видами транспорта.

#### **Используемые технологии**
* Unity3D
* RabbitMQ
* ARToolKit
* .Net Framework

#### **ARToolKit**
ARToolKit - библиотека дополненной реальности. Библиотека была интегрирована в Unity3D.

#### **Очередь сообщений**
Для связи с компонентами использовалась очередь сообщений RabbitMQ
Она была интегрирована с Unity3D.


#### **Компоненты распределенной системы**
* Автомобиль
* Поезд
* Самолет
* Точка (некий склад товаров)
* Товар
* Сборщик (из простых товаров собирает сложные)
* Сортировщик (устанавливает точки назначения, кто куда доставляет товар)
* Визуализатор (визуализирует взаимодействие компонент)




#### **Описание**

* Скрипты лежат в папке `Assets/scripts`
* Архив с проектом можно скачать с yandex диска по ссылке: https://yadi.sk/d/AgpFi2_i3LmpWx

Подробное описание можно найти в сборнике статей по ссылке http://conf-eekm.ru/wp-content/uploads/2016/01/molod-2017.pdf

* `АРХИТЕКТУРНЫЕ И ТЕХНИЧЕСКИЕ ОСОБЕННОСТИ
РАСПРЕДЕЛЕННОЙ СИСТЕМЫ ДОСТАВКИ ТОВАРОВ
РАЗЛИЧНЫМИ ВИДАМИ ТРАНСПОРТА` с.61

* `ПРОЕКТИРОВАНИЕ И СОЗДАНИЕ АРХИТЕКТУРЫ ДЛЯ
РАСПРЕДЕЛЕННОЙ СИСТЕМЫ ДОСТАВКИ ТОВАРОВ
НЕСКОЛЬКИМИ ВИДАМИ ТРАНСПОРТА` с.179
