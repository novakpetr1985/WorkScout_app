# Větve a vydávání WorkScout 1.0.0

## Chráněné větve

`dev`, `test` a `main` jsou pouze cílové větve pull requestů. Přímé commity,
push, rebase, cherry-pick a merge jsou zakázané. Ochranu musí vlastník nastavit
v GitHubu přes **Settings → Rules → Rulesets**; workflow ji samo nevynutí.

`feature/1.0.0` a `release/1.0.0` jsou pracovní větve. Release větev musí být
možné aktualizovat commitem verze před každým řízeným PR.

## Příprava release větve

1. Dokonči a ověř `feature/1.0.0` s verzí `1.0.0-feature`.
2. Z aktuální feature vytvoř `release/1.0.0`; její první stav zůstává
   `1.0.0-feature`.
3. Od tohoto bodu se funkční rozsah nemění. Přijímají se jen release opravy,
   testy a dokumentace.

## Povinný postup přes prostředí

### 1. DEV

Na `release/1.0.0` změň:

- `WorkScout_app.csproj`: `1.0.0-feature` → `1.0.0-DEV`
- `README.md`: úvodní uvedenou verzi na `1.0.0-DEV`
- `docs/CHANGELOG.md`: nadpis na `[1.0.0-DEV]`

Commitni a pushni release větev, potom vytvoř PR:

```text
release/1.0.0 → dev
```

### 2. TEST

Po schváleném merge do `dev` se vrať na `release/1.0.0` a změň stejné tři
hodnoty na `1.0.0-TEST`. Commitni, pushni a vytvoř PR:

```text
release/1.0.0 → test
```

### 3. MAIN

Po schváleném merge do `test` se vrať na `release/1.0.0` a nastav stabilní
`1.0.0` bez suffixu. V changelogu nahraď `připravováno` skutečným datem vydání.
Commitni, pushni a vytvoř PR:

```text
release/1.0.0 → main
```

Po merge aktualizuj lokální `main` a vytvoř anotovaný tag:

```powershell
git switch main
git pull --ff-only
git tag -a v1.0.0 -m "WorkScout 1.0.0"
git push origin v1.0.0
```

Tag spustí jedinou WorkScout pipeline. Ta zopakuje testy, ověří shodu tagu s
verzí bez suffixu a po ručním schválení environmentu `manual-approval` vytvoří
release artefakty.

## Opravy během povyšování

Funkční oprava vzniklá po PR do `dev` se nesmí ztratit. Provede se na pracovní
větvi odvozené z aktuální `release/1.0.0`, přes review se vrátí do release větve
a před postupem do dalšího prostředí se znovu synchronizuje také do již
povýšených větví samostatným PR. Chráněné větve se nikdy neopravují přímým commitem.

## Další verze

Po dokončení 1.0.0 vytvoř `feature/1.0.1` z aktuální `dev` a jako první změnu
nastav v `.csproj`, README a changelogu `1.0.1-feature`. Produkční opravy z
`main`, které nejsou v `dev`, se vrací samostatnou synchronizační větví a PR.

## GitHub Rulesets

Pro `dev`, `test` a `main` nastav:

- Require a pull request before merging.
- Require status check `WorkScout CI Pipeline / Build and test`.
- Require branches to be up to date before merging.
- Require conversation resolution.
- Block force pushes and branch deletion.
- Na `main` nastav schválení reviewerem, pokud je dostupný druhý vývojář.

Pro tagy `v*` zakaž přepsání a smazání. Pro skutečné pozastavení publikace
nastav **Settings → Environments → manual-approval → Required reviewers**.
