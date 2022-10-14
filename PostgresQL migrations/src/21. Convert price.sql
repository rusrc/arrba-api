-- FUNCTION: public."ConvertCurrencyPrice"(bigint, bigint, numeric, json)

-- DROP FUNCTION public."ConvertCurrencyPrice"(bigint, bigint, numeric, json);

CREATE OR REPLACE FUNCTION public."ConvertCurrencyPrice"(
	"TargetCurrencyID" bigint,
	"AdCurrencyID" bigint,
	"Price" double precision,
	"ExchangeRateRows" json)
    RETURNS integer
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$
--DECLARE "ExchangeRateRows" json := (select array_to_json(array_agg(T)) from (select * from "ExchangeRateTableView") T);
DECLARE "ExchangeRateRow" json;
DECLARE "Rate" real := 0;
DECLARE "FaceValue" integer := 0;
BEGIN
	IF("TargetCurrencyID" = "AdCurrencyID") THEN
		RETURN "Price";
	END IF;
	
	FOR "ExchangeRateRow" IN SELECT * FROM json_array_elements("ExchangeRateRows")
	  LOOP
		  IF (CAST( "ExchangeRateRow"->>'AdCurrencyID' AS BIGINT) = "AdCurrencyID" 
			  AND 
			  CAST( "ExchangeRateRow"->>'TargetCurrencyID' AS BIGINT) = "TargetCurrencyID") THEN
		    		
			"Rate" := CAST("ExchangeRateRow"->>'Rate' AS real);
			"FaceValue" := CAST("ExchangeRateRow"->>'FaceValue' as integer);
		  END IF;	
	  END LOOP;
	  
	  IF ("Rate" <> 0) AND ("FaceValue" <> 0) THEN
		--RAISE NOTICE 'Rate Is null ';
		RETURN "Price" * "Rate" / "FaceValue";
	  END IF;

	  RETURN "Price";
END
$BODY$;


-- ALTER FUNCTION public."ConvertCurrencyPrice"(bigint, bigint, double precision, json)
    -- OWNER TO postgres;
	
	
/*

--Usage 
--select public."ConvertCurrencyPrice"(4, 1, 100, (select array_to_json(array_agg(T)) from (select * from "ExchangeRateTableView") T));

Or

select public."ConvertCurrencyPrice"(4, 1, 100, 
'[{"TargetName":"RUR","TargetCurrencyID":4,"AdCurrencyName":"USD","AdCurrencyID":1,"FaceValue":1,"Rate":68.292},
{"TargetName":"RUR","TargetCurrencyID":4,"AdCurrencyName":"KZT","AdCurrencyID":2,"FaceValue":1,"Rate":0.205081},
{"TargetName":"RUR","TargetCurrencyID":4,"AdCurrencyName":"EUR","AdCurrencyID":3,"FaceValue":1,"Rate":77.1119},
{"TargetName":"KZT","TargetCurrencyID":2,"AdCurrencyName":"USD","AdCurrencyID":1,"FaceValue":1,"Rate":350},
{"TargetName":"KZT","TargetCurrencyID":2,"AdCurrencyName":"RUR","AdCurrencyID":4,"FaceValue":1,"Rate":5.15},
{"TargetName":"KZT","TargetCurrencyID":2,"AdCurrencyName":"EUR","AdCurrencyID":3,"FaceValue":1,"Rate":450},
{"TargetName":"USD","TargetCurrencyID":1,"AdCurrencyName":"KZT","AdCurrencyID":2,"FaceValue":1,"Rate":0.00285714},
{"TargetName":"USD","TargetCurrencyID":1,"AdCurrencyName":"EUR","AdCurrencyID":3,"FaceValue":1,"Rate":1.1277},
{"TargetName":"USD","TargetCurrencyID":1,"AdCurrencyName":"RUR","AdCurrencyID":4,"FaceValue":1,"Rate":0.0153846},
{"TargetName":"EUR","TargetCurrencyID":3,"AdCurrencyName":"USD","AdCurrencyID":1,"FaceValue":1,"Rate":0.88676},
{"TargetName":"EUR","TargetCurrencyID":3,"AdCurrencyName":"KZT","AdCurrencyID":2,"FaceValue":1,"Rate":0.0026178},
{"TargetName":"EUR","TargetCurrencyID":3,"AdCurrencyName":"RUR","AdCurrencyID":4,"FaceValue":1,"Rate":0.0136765}]'
);
*/


-------------------------------------------------------------------------------------------------------------
