# Ruční ověření diagnostického CI logu

Diagnostiku zkoušej pouze ručním spuštěním workflow `WorkScout CI Pipeline`.

1. Na GitHubu otevři **Actions → WorkScout CI Pipeline**.
2. Zvol **Run workflow** na pracovní větvi.
3. Zapni `simulate_failure` a spusť workflow.
4. Souhrnný job úmyslně skončí chybou pouze pro tento ruční běh.
5. Stáhni artifact `workscout-ci-error-log` z jobu `Diagnostic log`.
6. Ověř, že log obsahuje workflow, URL běhu, čas, ref, commit, SDK a podrobné
   výstupy restore, buildu a testů.
7. Ověř dostupnost coverage artefaktů unit a integration testů.
8. Spusť workflow znovu s `simulate_failure = false`; běžný CI musí projít.

Simulace nemění zdrojový kód, databázi ani release podmínky.
