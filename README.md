# testmatching
Сделал свою утилиту для генерации VMR файла.

Зачем? 

ModelMatchingMagic - программа хорошая, но результат выдаёт плачевный. Куча ненужных правил, правила пересекаются между собой, генерирует такие правила, что аирбас 320 может быть спокойно отрисован боингом 787 и прочее. Заметил, что при полётах  ватсим часто начались глюки с отрисовкой самолётов, он вроде есть, но впилот его не рисует... Взглянул на файл вмр - ужаснулся...


Что сделано:

пока консольное приложение с запросом пути или возможностью указать путь для поиска ливрей в конфиге (поиск в указанном каталоге и во всех вложенных).

за основу генерации взят пакет трафика AMBA_062 [Сергей] (FSLTL + AIG)

текущие правила выдёргивают все гражданские самолёты, за редким исключением, можете сами глянуть правила и вписать недостающие по примеру в файле templates.cfg

логика работы - поиск совпадений по имени ливреи, поэтому может не работать с некоторыми дефолтными ливреями и трафиком  IVAO.


Что планируется сделать:

добавить GUI 

добавить возможность работы со всеми типами файлов aircraft.cfg (их структурой), чтобы искать не только в трафике от Сергея, но и во всех возможных вариантах трафика и ливрей

добавить возможность сравнивать наличие ливрей по типу (например если есть трафик AFL airbus 320 но нет AFL boeing 737, то сгенерируется правило, которое будет отрисовывать боинг аирбасом AFL)

В общем, выкладываю на ваше обозрение

Вопросы и пожелания приветствуются)
