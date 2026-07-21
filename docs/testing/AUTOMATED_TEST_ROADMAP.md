# Roadmap automatických testů

## Baseline 1.0.0-feature

- 5 unit testů.
- 3 automatické integrační testy.
- Unit testy běží při každém pushi, PR, tagu i ručním spuštění.
- Integrační testy běží při PR, tagu a ručním spuštění.
- Obě sady vytvářejí Cobertura coverage artefakt.
- Testy nepoužívají reálný Gmail ani pracovní portál.

## Současné pokrytí

| Oblast | Stav | Poznámka |
|---|---|---|
| Verze aplikace | Hotovo | podporované suffixy a zobrazovaný název |
| DPAPI | Hotovo | encrypt/decrypt round-trip bez otevřeného textu |
| Testovací zdroj | Hotovo | data, povinná pole a cancellation |
| Portály | Základ | událost změny výběru |
| EF migrace | Hotovo | vytvoření schématu a seed portálů |
| Žádosti a zprávy | Základ | persistence a relace |
| Deduplikace URL | Hotovo | ověřený unikátní index |
| Gmail IMAP/SMTP | Neautomatizovat online | produkční účet nesmí být součástí CI |
| WPF UI | Chybí | doplnit ViewModel testy a manuální smoke scénáře |

## Priority po 1.0.0

1. Testy validačních stavů setup wizardu bez skutečné sítě.
2. Testy filtrování nabídek a chyb jednotlivých `IJobSource` adaptérů.
3. Testy stavového toku žádosti a povinného schválení před odesláním.
4. Testy parsování e-mailových hlaviček a ochrany proti automatickým smyčkám.
5. Coverage baseline kritických služeb; triviální DTO a generovaný kód se do
   cíle nemají uměle započítávat.
