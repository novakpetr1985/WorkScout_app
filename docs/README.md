# 📚 Dokumentace WorkScout

Tento adresář je rozcestník dokumentace aplikace WorkScout. Aktuální verze
`1.0.0-feature` je startovací Windows release s demonstračními pracovními nabídkami,
lokální SQLite databází a připraveným základem pro Gmail a budoucí zdroje inzerátů.

## 🧭 Přehled

| Dokument | Obsah |
|---|---|
| [`../README.md`](../README.md) | spuštění, funkce, omezení a architektura |
| [`CHANGELOG.md`](CHANGELOG.md) | změny aktuální a vydaných verzí |
| [`ROADMAP.md`](ROADMAP.md) | produktová vize a plán dalších etap |
| [`architecture/FEATURE_MAP.md`](architecture/FEATURE_MAP.md) | stav funkcí a dohledatelnost v kódu |
| [`git/BRANCH_AND_RELEASE_WORKFLOW.md`](git/BRANCH_AND_RELEASE_WORKFLOW.md) | větve, PR, verzování a tag `v1.0.0` |
| [`testing/AUTOMATED_TEST_ROADMAP.md`](testing/AUTOMATED_TEST_ROADMAP.md) | automatizované testy a jejich priority |
| [`testing/MANUAL_TEST_SCENARIOS.md`](testing/MANUAL_TEST_SCENARIOS.md) | ruční kontrola před vydáním |
| [`ci/CI_DIAGNOSTIC_TEST.md`](ci/CI_DIAGNOSTIC_TEST.md) | ověření chybového artefaktu pipeline |
| [`releases/v1.0.0.md`](releases/v1.0.0.md) | připravený popis prvního stabilního releasu |

## 🧹 Pravidlo údržby

Každá informace má jeden hlavní dokument. Ostatní soubory na ni pouze odkazují,
aby se postup vydání, testovací checklist ani omezení aplikace nerozcházely.

Při povýšení `release/1.0.0` se současně aktualizuje verze v
`WorkScout_app.csproj`, úvodním README a changelogu. Přesný postup je uvedený v
[`git/BRANCH_AND_RELEASE_WORKFLOW.md`](git/BRANCH_AND_RELEASE_WORKFLOW.md).
