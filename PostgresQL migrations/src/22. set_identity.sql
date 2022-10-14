-- Set identity
select setval('"Type_ID_seq"', (select max("ID") from "Type")); 
select setval('"SuperCategories_ID_seq"', (select max("ID") from "SuperCategories")); 
select setval('"Categories_ID_seq"', (select max("ID") from "Categories")); 
select setval('"Countries_ID_seq"', (select max("ID") from "Countries")); 
select setval('"Regions_ID_seq"', (select max("ID") from "Regions")); 
select setval('"Cities_ID_seq"', (select max("ID") from "Cities")); 
select setval('"Currencies_ID_seq"', (select max("ID") from "Currencies")); 
select setval('"Brands_ID_seq"', (select max("ID") from "Brands")); 
select setval('"Type_ID_seq"', (select max("ID") from "Type")); 
select setval('"Model_ID_seq"', (select max("ID") from "Model"));  
select setval('"CheckBoxGroups_ID_seq"', (select max("ID") from "CheckBoxGroups")); 
select setval('"CurrencyRates_ID_seq"', (select max("ID") from "CurrencyRates")); 
select setval('"PropertyGroups_ID_seq"', (select max("ID") from "PropertyGroups"));
select setval('"Properties_ID_seq"', (select max("ID") from "Properties"));
select setval('"SelectOptions_ID_seq"', (select max("ID") from "SelectOptions"));


-------------------------------------------------------------------------------------------------------------