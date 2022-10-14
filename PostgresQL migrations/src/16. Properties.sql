--select * from "Properties" --96
insert into "Properties" ("ID",  "PropertyGroupID", "Name", "Description", "ControlType", "ActiveStatus", "NameMultiLangJson", "UnitMeasure")
values
(1 ,5 ,'Color' ,'Цвет' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Цвет кузова"},{"LangName":"kk","Value":"Түсі"},{"LangName":"en","Value":"Color"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(2 ,5 ,'Bodycar' ,'Кузов' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Кузов"},{"LangName":"kk","Value":"Кузов"},{"LangName":"en","Value":"carcase"},{"LangName":"de","Value":"carcase"},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(3 ,5 ,'EngineType' ,'Тип двигателя' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Тип двигателя"},{"LangName":"kk","Value":"қозғалтқыш түрі"},{"LangName":"en","Value":"Engine type"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(4 ,5 ,'mileage' ,'пробег (км)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"пробег (км)"},{"LangName":"kk","Value":"жорық (км)"},{"LangName":"en","Value":"mileage (km)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','км'),
(5 ,5 ,'FuelType' ,'Вид топлива' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Вид топлива"},{"LangName":"kk","Value":"Жанармай түрі"},{"LangName":"en","Value":"Fuel type"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(6 ,5 ,'EngineValue' ,'Объем двигателя (см3)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"Объем двигателя (см3)"},{"LangName":"kk","Value":"қозғалтқыш  көлемі (см3)"},{"LangName":"en","Value":"Engine value (cm3)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','см3'),
(7 ,5 ,'Power' ,'Мощность (л.с.)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"Мощность (л.с.)"},{"LangName":"kk","Value":"Күш (л.с.)"},{"LangName":"en","Value":"Power (l.s)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','л.с.'),
(8 ,5 ,'Turbocharging' ,'Турбонаддув' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Турбонаддув"},{"LangName":"kk","Value":""},{"LangName":"en","Value":"Turbocharging"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(9 ,5 ,'NumberOfCylinders' ,'Количество цилиндров' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Количество цилиндров"},{"LangName":"kk","Value":"Цилиндр саны"},{"LangName":"en","Value":"Number Of Cylinders"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(10 ,5 ,'NumberOfCycles' ,'Количество тактов ' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"количество тактов"},{"LangName":"kk","Value":"такт саны"},{"LangName":"en","Value":"number of cycles"},{"LangName":"de","Value":"die Anzahl der Zyklen"},{"LangName":"he","Value":"מספר מחזורי"}]',null),
(11 ,2 ,'TrackWidth' ,'Ширина гусеницы ' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Ширина гусеницы"},{"LangName":"kk","Value":"қырықбуын ені"},{"LangName":"en","Value":"caterpillar width"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(12 ,2 ,'Width' ,'Ширина (м)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"Ширина (м)"},{"LangName":"kk","Value":"Ені (м)"},{"LangName":"en","Value":"Width (m)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','м'),
(13 ,2 ,'Length' ,'Длина (м)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"Длина (м)"},{"LangName":"kk","Value":"Ұзындығы (м)"},{"LangName":"en","Value":"Length (m)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','м'),
(14 ,2 ,'Height' ,'Высота (м)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"Высота (м)"},{"LangName":"kk","Value":"Биіктігі (м)"},{"LangName":"en","Value":"Height (m)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','м'),
(15 ,5 ,'LoadCapacity' ,'Грузоподъемность (кг)' ,0 ,1 ,'[{"LangName":"ru-RU","Value":"Грузоподъемность (кг)"},{"LangName":"kk","Value":"жүк көтергіштігі (кг)"},{"LangName":"en","Value":"carrying capacity (kg)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]','кг'),
(16 ,2 ,'TowHook' ,'буксировочный крюк' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"буксировочный крюк"},{"LangName":"kk","Value":"тіркегіш ілгек"},{"LangName":"en","Value":"tow hook"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(17 ,2 ,'TemperatureSensor' ,'датчик температуры' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"датчик температуры"},{"LangName":"kk","Value":"температура датчигі"},{"LangName":"en","Value":"temperature sensor"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(18 ,2 ,'FuelSensor' ,'датчик топлива' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"датчик топлива"},{"LangName":"kk","Value":"жанармай датчигі"},{"LangName":"en","Value":"Fuel sensor"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(19 ,5 ,'ReverseMovement' ,'задний ход' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"задний ход"},{"LangName":"kk","Value":"Артқы жүріс"},{"LangName":"en","Value":"Reverse movement"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(20 ,5 ,'HeatedHandles' ,'подогрев рукояток' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Подогрев рукояток"},{"LangName":"kk","Value":"Тұтқа жылытқыш"},{"LangName":"en","Value":"Heated handles"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(21 ,2 ,'BackMirrors' ,'зеркала заднего вида' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"зеркала заднего вида"},{"LangName":"kk","Value":"Артты көрі айнасы"},{"LangName":"en","Value":"Back mirrors"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(22 ,2 ,'MirrorHeat' ,'подогрев зеркал' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Подогрев зеркал"},{"LangName":"kk","Value":"Айна жылыту"},{"LangName":"en","Value":"Mirror heating"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(23 ,5 ,'Speedometer' ,'спидометр' ,3 ,1 ,null,null),
(24 ,5 ,'Tachometer' ,'тахометр' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Тахометр"},{"LangName":"kk","Value":"Тахометр"},{"LangName":"en","Value":"Tachometer"},{"LangName":"de","Value":"Tachometer"},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(25 ,5 ,'ElectricStarter' ,'электростартер' ,3 ,1 ,null,null),
(26 ,5 ,'HourMeter' ,'счетчик моточасов' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"счетчик моточасов"},{"LangName":"kk","Value":"мотосағат есептеуші"},{"LangName":"en","Value":"hour meter"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(27 ,1 ,'Cooling' ,'Охлаждение' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"Охлаждение"},{"LangName":"kk","Value":"Суыту"},{"LangName":"en","Value":"Cooling"},{"LangName":"de","Value":"Kühlung"},{"LangName":"he","Value":"קירור"}]',null),
(28 ,1 ,'DriveUnit' ,'Привод' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"привод"},{"LangName":"kk","Value":"привод"},{"LangName":"en","Value":"drive unit"},{"LangName":"de","Value":"Antrieb"},{"LangName":"he","Value":"כונן"}]',null),
(29 ,1 ,'PPC' ,'КПП' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"коробка передачь"},{"LangName":"kk","Value":""},{"LangName":"en","Value":"PPC"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(30 ,5 ,'ABS' ,'ABS' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"ABS"},{"LangName":"kk","Value":"ABS (тоқтатудың антибұғаттау жүйесі)"},{"LangName":"en","Value":"ABS (Anti-lock braking system)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(31 ,5 ,'Signaling' ,'сигнализация' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"сигнализация"},{"LangName":"kk","Value":"дабылқаққыш"},{"LangName":"en","Value":"Signaling"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(32 ,5 ,'ISG' ,'ISG (стоп-старт)' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"ISG (стоп-старт)"},{"LangName":"kk","Value":"ISG (тоқта-баста)"},{"LangName":"en","Value":"ISG (stop-start)"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(33 ,2 ,'LuggageTrunk' ,'багажный кофр' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Багажный кофр"},{"LangName":"kk","Value":"Багаж сандығы"},{"LangName":"en","Value":"Luggage trunk"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(34 ,2 ,'Windshield' ,'ветровое стекло' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Ветровое стекло"},{"LangName":"kk","Value":"Жел әйнегі"},{"LangName":"en","Value":"Windscreen"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(35 ,1 ,'Base' ,'База' ,2 ,1 ,'[{"LangName":"ru-RU","Value":"База"},{"LangName":"kk","Value":"База"},{"LangName":"en","Value":"Base"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(36 ,5 ,'Exists' ,'В наличие' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"В наличие"},{"LangName":"kk","Value":"қолда бар"},{"LangName":"en","Value":"Exists"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(37 ,5 ,'ForOrder' ,'На заказ' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"На заказ"},{"LangName":"kk","Value":"Тапсырысқа"},{"LangName":"en","Value":"To order"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(38 ,5 ,'ButtomProtection' ,'защита днища' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Защита днища"},{"LangName":"kk","Value":"Түбін қорғағыш"},{"LangName":"en","Value":"Buttom protection"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(39 ,5 ,'DirectionIndicators' ,'указатели поворота' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Указатели поворота"},{"LangName":"kk","Value":"Бұрылысты көрсеткіш"},{"LangName":"en","Value":"Direction indicators"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(41 ,2 ,'alloy_wheels  ' ,'Литые диски' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Литые диски"},{"LangName":"kk","Value":"Литые диски"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(42 ,2 ,'toning' ,'тонировка' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"тонировка"},{"LangName":"kk","Value":"тонировка"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(43 ,2 ,'manhole' ,'Люк' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"Люк"},{"LangName":"kk","Value":"Люк"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(44 ,2 ,'panoramic_roof' ,'панорамная крыша' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"панорамная крыша"},{"LangName":"kk","Value":"панорамная крыша"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(45 ,2 ,'Bullbar' ,'кенгурятник' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"кенгурятник"},{"LangName":"kk","Value":"кенгурятник"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(46 ,2 ,'spoiler' ,'спойлер' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"спойлер"},{"LangName":"kk","Value":"спойлер"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(47 ,2 ,'body_kit' ,'обвес' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"обвес"},{"LangName":"kk","Value":"обвес"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(48 ,2 ,'winch' ,'лебёдка' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"лебёдка"},{"LangName":"kk","Value":"лебёдка"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(49 ,2 ,'car_visor' ,'ветровики' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"ветровики"},{"LangName":"kk","Value":"ветровики"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(50 ,2 ,'railing' ,'рейлинги' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"рейлинги"},{"LangName":"kk","Value":"рейлинги"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(51 ,2 ,'trunk_car' ,'багажник (авто)' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"багажник"},{"LangName":"kk","Value":"багажник"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(52 ,2 ,'turnbuckle' ,'фаркоп' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"фаркоп"},{"LangName":"kk","Value":"фаркоп"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(53 ,3 ,'xenon' ,'ксенон' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"ксенон"},{"LangName":"kk","Value":"ксенон"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(54 ,3 ,'Egolight' ,'биксенон' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"биксенон"},{"LangName":"kk","Value":"биксенон"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(55 ,3 ,'crystal_optics' ,'хрустальная оптика' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"хрустальная оптика"},{"LangName":"kk","Value":"хрустальная оптика"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(56 ,3 ,'linsed_optics' ,'линзованная оптика' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"линзованная оптика"},{"LangName":"kk","Value":"линзованная оптика"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(57 ,3 ,'Daytime_Running_Lights' ,'дневные ходовые огни' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"дневные ходовые огни"},{"LangName":"kk","Value":"дневные ходовые огни"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(58 ,3 ,'fog_lights' ,'противотуманки' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"противотуманки"},{"LangName":"kk","Value":"противотуманки"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(59 ,3 ,'headlight_washer' ,'омыватель фар' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"омыватель фар"},{"LangName":"kk","Value":"омыватель фар"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(60 ,3 ,'headlights_corrector' ,'корректор фар' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"корректор фар"},{"LangName":"kk","Value":"корректор фар"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(61 ,3 ,'heated_mirrors  ' ,'обогрев зеркал' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"обогрев зеркал"},{"LangName":"kk","Value":"обогрев зеркал"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(62 ,4 ,'velours' ,'велюр' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"велюр"},{"LangName":"kk","Value":"велюр"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(63 ,4 ,'leather' ,'кожа' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"кожа"},{"LangName":"kk","Value":"кожа"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(64 ,4 ,'wood' ,'дерево' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"дерево"},{"LangName":"kk","Value":"дерево"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(65 ,4 ,'alcantara' ,'алькантара' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"алькантара"},{"LangName":"kk","Value":"алькантара"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(66 ,4 ,'combined' ,'комбинированный' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"комбинированный"},{"LangName":"kk","Value":"комбинированный"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(67 ,4 ,'curtains' ,'шторки' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"шторки"},{"LangName":"kk","Value":"шторки"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(68 ,6 ,'audio_system' ,'аудиосистема' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"аудиосистема"},{"LangName":"kk","Value":"аудиосистема"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(69 ,6 ,'imbedded_phone' ,'встроенный телефон' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"встроенный телефон"},{"LangName":"kk","Value":"встроенный телефон"},{"LangName":"en","Value":""},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(70 ,6 ,'bluetooth' ,'bluetooth' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"bluetooth"},{"LangName":"kk","Value":"bluetooth"},{"LangName":"en","Value":"bluetooth"},{"LangName":"de","Value":"bluetooth"},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(71 ,6 ,'CD' ,'CD' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"CD"},{"LangName":"kk","Value":"CD"},{"LangName":"en","Value":"CD"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(72 ,6 ,'CD-changer' ,'CD-changer' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"CD-чейнджер"},{"LangName":"kk","Value":"CD-чейнджер"},{"LangName":"en","Value":"CD-changer"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(73 ,6 ,'MP3' ,'MP3' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"MP3"},{"LangName":"kk","Value":"MP3"},{"LangName":"en","Value":"MP3"},{"LangName":"de","Value":"MP3"},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(74 ,6 ,'USB' ,'USB' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"USB"},{"LangName":"kk","Value":"USB"},{"LangName":"en","Value":"USB"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(75 ,6 ,'DVD' ,'DVD' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"DVD"},{"LangName":"kk","Value":"DVD"},{"LangName":"en","Value":"DVD"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(76 ,6 ,'DVD-changer' ,' DVD-changer' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"DVD-чейнджер"},{"LangName":"kk","Value":"DVD-чейнджер"},{"LangName":"en","Value":" DVD-changer"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(77 ,6 ,'subBuffer' ,'сабвуфер' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"сабвуфер"},{"LangName":"kk","Value":"сабвуфер"},{"LangName":"en","Value":"сабвуфер"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(78 ,7 ,'power_steering' ,'ГУР' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"ГУР"},{"LangName":"kk","Value":"ГУР"},{"LangName":"en","Value":"power steering"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(80 ,7 ,'SRS' ,'SRS' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"SRS"},{"LangName":"kk","Value":"SRS"},{"LangName":"en","Value":"SRS"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(81 ,7 ,'winter_mode' ,'зимний режим' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"зимний режим"},{"LangName":"kk","Value":"қысқы режим"},{"LangName":"en","Value":"winter mode"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(82 ,7 ,'sport_mode' ,'спортивный режим' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"спортивный режим"},{"LangName":"kk","Value":"Спорттық режимі"},{"LangName":"en","Value":"sport mode"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(83 ,7 ,'Turbo' ,'турбонаддув' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"турбонаддув"},{"LangName":"kk","Value":"турбонаддув"},{"LangName":"en","Value":"Turbo"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(84 ,1 ,'turbotimer' ,'турботаймер' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"турботаймер"},{"LangName":"kk","Value":"турботаймер"},{"LangName":"en","Value":"turbo timer"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(86 ,7 ,'autostart' ,'autostart' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"автозапуск"},{"LangName":"kk","Value":"Авто ойнату"},{"LangName":"en","Value":"autostart"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(87 ,7 ,'immobilizer' ,'immobilizer' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"иммобилайзер"},{"LangName":"kk","Value":"иммобилайзер"},{"LangName":"en","Value":"immobilizer"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(88 ,7 ,'keyless_access' ,'бесключевой доступ' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"бесключевой доступ"},{"LangName":"kk","Value":"Тез қысатын қол"},{"LangName":"en","Value":"keyless access"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(89 ,7 ,'full_power' ,'полный электропакет' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"полный электропакет"},{"LangName":"kk","Value":"толық қуат"},{"LangName":"en","Value":"full power"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(90 ,7 ,'center_lock' ,'центрозамок' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"центрозамок"},{"LangName":"kk","Value":"центрозамок"},{"LangName":"en","Value":"центрозамок"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(91 ,7 ,'air_conditioning' ,'кондиционер' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"кондиционер"},{"LangName":"kk","Value":"ауаны"},{"LangName":"en","Value":"air conditioning"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(92 ,7 ,'climate_control' ,'климат-контроль' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"климат-контроль"},{"LangName":"kk","Value":"климат-бақылау"},{"LangName":"en","Value":"climate control"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(93 ,7 ,'Cruise_control' ,'круиз-контроль' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"круиз-контроль"},{"LangName":"kk","Value":"круиз бақылау"},{"LangName":"en","Value":"Cruise control"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(94 ,7 ,'navigation_system' ,'навигационная система' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"навигационная система"},{"LangName":"kk","Value":"навигация жүйесі"},{"LangName":"en","Value":"navigation system"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(95 ,7 ,'Multifunction ' ,'Multifunction ' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"мультируль"},{"LangName":"kk","Value":"мультируль"},{"LangName":"en","Value":"Multifunction "},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(96 ,7 ,'heated_steering_wheel' ,'подогрев руля' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"подогрев руля"},{"LangName":"kk","Value":"қыздырылған руль дөңгелегі"},{"LangName":"en","Value":"heated steering wheel"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(97 ,7 ,'seat_ventilation' ,'вентиляция сидений' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"вентиляция сидений"},{"LangName":"kk","Value":"желдету дисктері"},{"LangName":"en","Value":"seat ventilation"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(98 ,7 ,'seat_memory' ,'память сидений' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"память сидений"},{"LangName":"kk","Value":"орындық жад"},{"LangName":"en","Value":"seat memory"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null),
(100 ,7 ,'steering_memory' ,'память руля' ,3 ,1 ,'[{"LangName":"ru-RU","Value":"память руля"},{"LangName":"kk","Value":"рульдік жады"},{"LangName":"en","Value":"steering memory"},{"LangName":"de","Value":""},{"LangName":"he","Value":""},{"LangName":"hy-AM","Value":""}]',null)
ON CONFLICT ("ID") DO UPDATE 
  SET "ActiveStatus" = excluded."ActiveStatus", 
      "ControlType" = excluded."ControlType",
	  "Description" = excluded."Description",
	  "Name" = excluded."Name",
	  "NameMultiLangJson" = excluded."NameMultiLangJson",
	  "PropertyGroupID" = excluded."PropertyGroupID",
	  "UnitMeasure" = excluded."UnitMeasure";


-------------------------------------------------------------------------------------------------------------