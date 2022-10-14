create view public."ExchangeRateTableView"
as
select c."Name" "TargetName", cr."CurrencyBaseRateID" "TargetCurrencyID",
 c2."Name" "AdCurrencyName", cr."CurrencyID" "AdCurrencyID", cr."FaceValue", 
 cr."Rate" from "Currencies" c
inner join "CurrencyRates" cr on c."ID" = cr."CurrencyBaseRateID"
inner join "Currencies" c2 on c2."ID" = cr."CurrencyID";


-------------------------------------------------------------------------------------------------------------