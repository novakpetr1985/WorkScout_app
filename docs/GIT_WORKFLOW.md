# Git workflow a verzování

Veškeré změny chráněných větví procházejí pull requestem. Přímý push, force push
a mazání větví `dev`, `test`, `release/*` a `main` mají být v GitHub rulesetech
zakázané.

## Větve a verze

Zdroj pravdy je `Version.props`. `VersionPrefix` obsahuje připravovanou verzi a
CI doplní kanál podle větve:

| Větev | BuildChannel | Výsledná verze |
|---|---|---|
| `feature/1.0.0` | `feature` | `1.0.0-feature` |
| `dev` | `dev` | `1.0.0-dev` |
| `test` | `test` | `1.0.0-test` |
| `release/1.0.0` | `rc` | `1.0.0-rc` |
| `main` | `stable` | `1.0.0` |

Stabilní `1.0.0` tedy nikdy nevznikne na feature, dev, test ani release větvi.
Vznikne pouze sestavením commitu v `main` nebo tagu `v1.0.0`.

Pro lokální zobrazení jiného kanálu zkopíruj `Version.local.props.example` jako
`Version.local.props` a změň `BuildChannel`. Soubor je v `.gitignore`, takže
nevytváří konflikty mezi větvemi. Stejného výsledku lze dosáhnout příkazem:

```powershell
dotnet build WorkScout.slnx -p:BuildChannel=dev
```

## Povolený průchod verze 1.0.0

1. Vývoj probíhá na `feature/1.0.0`, vytvořené z aktuální `dev`.
2. PR: `feature/1.0.0` → `dev`.
3. PR: `dev` → `test`.
4. Z `main` se vytvoří prázdná cílová větev `release/1.0.0` a odešle se na origin.
5. PR: `test` → `release/1.0.0`.
6. Opravy kandidáta probíhají přes `fix/*` → `release/1.0.0`.
7. PR: `release/1.0.0` → `main`.
8. Po merge se na commitu v `main` ručně vytvoří a odešle anotovaný tag `v1.0.0`.
9. Tag spustí workflow `Create release`, které ověří verzi, spustí testy a vytvoří GitHub Release.
10. PR: `main` → `dev`, aby se vydané opravy vrátily do vývojové větve.
11. Z aktuální `dev` se vytvoří `feature/1.0.1`; v ní se změní `VersionPrefix` na `1.0.1`.

Příklad vytvoření release větve bez přímé změny jejího obsahu:

```powershell
git switch main
git pull --ff-only
git switch -c release/1.0.0
git push -u origin release/1.0.0
```

Příklad tagu po schváleném PR do `main`:

```powershell
git switch main
git pull --ff-only
git tag -a v1.0.0 -m "WorkScout 1.0.0"
git push origin v1.0.0
```

## Povolené směry PR

- `feature/*` nebo `chore/*` → `dev`
- `dev` → `test`
- `test` → `release/*`
- `fix/*` nebo `hotfix/*` → `release/*`
- `release/*` → `main`
- `main` → `dev` pro synchronizaci po vydání

CI jiný směr odmítne. Přímé `feature/*` → `main` ani `dev` → `main` není povolené.

## Doporučené GitHub rulesety

V **Settings → Rules → Rulesets** nastav pro `dev`, `test`, `release/*` a `main`:

- Require a pull request before merging.
- Require status checks to pass: `Build and test`.
- Require branches to be up to date before merging.
- Require conversation resolution.
- Block force pushes.
- Restrict deletions.
- Na `main` a `release/*` zapni alespoň jedno schválení, pokud je dostupný druhý reviewer.

Pro samostatného vývojáře lze ponechat počet povinných schválení na nule, ale PR
a úspěšný CI check musí zůstat povinné. Pro tagy `v*` je vhodné přidat samostatný
ruleset, který zakáže přepsání a smazání tagu.
