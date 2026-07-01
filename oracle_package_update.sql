-- =============================================================
--  SL_A1_ANTRAG  –  Package-Update
--  Änderungen:
--    + ANTRAEGE_OFFEN_SQL  (Filter: offene Anträge)
--    + ANTRAEGE_ALLE_SQL   (Filter: alle bis 2 Jahre zurück)
--    + MA_SELECT_SQL       (Mitarbeiterliste)
--    + GEPA_SELECT_SQL     (Kundenliste)
--    - a1_lesen_schreiben_jn  (Legacy, Body auskommentiert)
--    - a1_ma_anzeige          (Staging-Tabelle SL_A1_ANTRAG_MA, ungenutzt)
--    - a1_gepa_volltext        (Staging-Tabelle SL_A1_ANTRAG_GEPA, ungenutzt)
--    - a1_gepa_ansprech        (Staging-Tabelle SL_A1_ANTRAG_ANSPR, ungenutzt)
-- =============================================================
-- Zuerst Header (Specification), dann Body kompilieren.
-- =============================================================

CREATE OR REPLACE EDITIONABLE PACKAGE "SIVAS"."SL_A1_ANTRAG" IS

  -- Author  : HAAS

  -- ── Neue SQL-Getter-Funktionen (Logik auf DB-Seite) ─────────
  function antraege_offen_sql return varchar2;
  function antraege_alle_sql  return varchar2;
  function ma_select_sql      return varchar2;
  function gepa_select_sql    return varchar2;

  -- ── DML-Prozeduren ──────────────────────────────────────────
  procedure a1_anlegen(
    i_pers_nr       in number,
    i_fam_name      in varchar2,
    i_name_vorname  in varchar2,
    i_von           in varchar2,
    i_bis           in varchar2,
    i_kdnr          in number default null,
    i_firma         in varchar2,
    i_strasse       in varchar2,
    i_plz           in varchar2,
    i_ort           in varchar2,
    i_land          in varchar2,
    i_angelegt_von  in varchar2);

  procedure a1_bearbeiten(
    i_lfdnr    in number,
    i_von      in varchar2,
    i_bis      in varchar2,
    i_strasse  in varchar2,
    i_plz      in varchar2,
    i_ort      in varchar2,
    i_user     in varchar2,
    i_newstatus in varchar2);

  procedure update_delete(i_lfdnr in number, i_function in varchar2, i_user in varchar2);
  procedure a1_workflow_status(i_lfdnr in number, i_oldstatus in varchar2, i_newstatus in varchar2);

END SL_A1_ANTRAG;
/

-- =============================================================

CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SIVAS"."SL_A1_ANTRAG" IS

------------------------------------------------------------------------
-- SQL-Getter: Gibt SELECT-Strings zurück, damit die App kein SQL
-- hardcodieren muss. Änderungen hier wirken sofort ohne Neukompilierung.
------------------------------------------------------------------------

function antraege_offen_sql return varchar2 is
begin
  return
    'SELECT LFDNR, PERS_NR, FAM_NAME, NAME_VORNAME, VON, BIS, '
    || 'KDNR, FIRMA, STRASSE, PLZ, ORT, LAND, ANSPRECH_NAME, ANSPRECH_VORNAME, '
    || 'STATUS, BEANTRAGT_JN, BEANTRAGT_AM, BEANTRAGT_VON, '
    || 'GENEHMIGT_JN, GENEHMIGT_AM, GENEHMIGT_VON, '
    || 'VORL_ERH_JN, VORL_ERH_AM, VORL_ERH_VON, '
    || 'ANGELEGT_AM, ANGELEGT_VON, BEARBEITET_AM, BEARBEITET_VON '
    || 'FROM SIVAS.SL_A1_ANTRAG_TAB '
    || 'WHERE STATUS NOT IN (''85 Stornierung beantragt'',''90 Antrag ungültig (löschen)'') '
    || 'ORDER BY LFDNR DESC';
end;

function antraege_alle_sql return varchar2 is
begin
  return
    'SELECT LFDNR, PERS_NR, FAM_NAME, NAME_VORNAME, VON, BIS, '
    || 'KDNR, FIRMA, STRASSE, PLZ, ORT, LAND, ANSPRECH_NAME, ANSPRECH_VORNAME, '
    || 'STATUS, BEANTRAGT_JN, BEANTRAGT_AM, BEANTRAGT_VON, '
    || 'GENEHMIGT_JN, GENEHMIGT_AM, GENEHMIGT_VON, '
    || 'VORL_ERH_JN, VORL_ERH_AM, VORL_ERH_VON, '
    || 'ANGELEGT_AM, ANGELEGT_VON, BEARBEITET_AM, BEARBEITET_VON '
    || 'FROM SIVAS.SL_A1_ANTRAG_TAB '
    || 'WHERE ANGELEGT_AM >= SYSDATE - 730 '
    || 'ORDER BY LFDNR DESC';
end;

function ma_select_sql return varchar2 is
begin
  return
    'SELECT t.persnr, t.familienname AS FAM_NAME, t.vorname AS NAME_VORNAME '
    || 'FROM SIVAS.PDE_PERSSTAMM t '
    || 'WHERE NVL(ausblenden_jn,''N'') = ''N'' '
    || 'AND (TO_CHAR(austrdat,''yyyy'') >= TO_CHAR(SYSDATE,''yyyy'') OR austrdat IS NULL) '
    || 'AND (TO_CHAR(eintrdat,''YYYY'') <= TO_CHAR(SYSDATE,''yyyy'')) '
    || 'ORDER BY t.familienname';
end;

function gepa_select_sql return varchar2 is
begin
  return
    'SELECT t.kdnr, NVL(t.name1,t.name2) AS FIRMA, t.strasse, t.ort, t.land, t.plz '
    || 'FROM SIVAS.GEPA t '
    || 'WHERE t.gepa_c1 IN (''K'',''L'',''I'') '
    || 'AND t.land NOT IN (''Deutschland'') '
    || 'AND t.kz_aktiv = ''J'' '
    || 'ORDER BY NVL(t.name1,t.name2)';
end;

------------------------------------------------------------------------
-- DML-Prozeduren (unverändert)
------------------------------------------------------------------------

procedure a1_anlegen(
  i_pers_nr       in number,
  i_fam_name      in varchar2,
  i_name_vorname  in varchar2,
  i_von           in varchar2,
  i_bis           in varchar2,
  i_kdnr          in number default null,
  i_firma         in varchar2,
  i_strasse       in varchar2,
  i_plz           in varchar2,
  i_ort           in varchar2,
  i_land          in varchar2,
  i_angelegt_von  in varchar2) is

x_von          date;
x_bis          date;
x_angelegt_am  date;

cursor c_lfdnr is
  select max(t.lfdnr)+1 from sl_a1_antrag_tab t;

x_lfdnr number;

begin

x_von         := trunc(to_date(i_von,'DD.MM.YY'));
x_bis         := trunc(to_date(i_bis,'DD.MM.YY'));
x_angelegt_am := trunc(to_date(sysdate,'DD.MM.YY'));

open c_lfdnr;
  fetch c_lfdnr into x_lfdnr;
close c_lfdnr;

insert into sl_a1_antrag_tab(
  pers_nr, fam_name, name_vorname, von, bis, kdnr,
  firma, strasse, plz, ort, land,
  angelegt_am, angelegt_von,
  beantragt_jn, genehmigt_jn,
  lfdnr, status, bearbeitet_am, bearbeitet_von)
values(
  i_pers_nr, i_fam_name, i_name_vorname, x_von, x_bis, i_kdnr,
  replace(i_firma,   '''',''),
  replace(i_strasse, '''',''),
  i_plz, i_ort, i_land,
  x_angelegt_am, i_angelegt_von,
  'N', 'N',
  x_lfdnr, '20 in Bearbeitung Perso', x_angelegt_am, i_angelegt_von);
commit;

a1_workflow_status(
  i_lfdnr     => x_lfdnr,
  i_oldstatus => '10 neuer Antrag',
  i_newstatus => '20 in Bearbeitung Perso');

end;

------------------------------------------------------------------------

procedure a1_bearbeiten(
  i_lfdnr     in number,
  i_von       in varchar2,
  i_bis       in varchar2,
  i_strasse   in varchar2,
  i_plz       in varchar2,
  i_ort       in varchar2,
  i_user      in varchar2,
  i_newstatus in varchar2) is

cursor c_status_old is
  select a.status from sl_a1_antrag_tab a where a.lfdnr = i_lfdnr;

x_status_old varchar2(100);
x_von date;
x_bis date;

begin

x_von := trunc(to_date(i_von,'DD.MM.YY'));
x_bis := trunc(to_date(i_bis,'DD.MM.YY'));

open c_status_old;
  fetch c_status_old into x_status_old;
close c_status_old;

update sl_a1_antrag_tab a
set a.von = x_von, a.bis = x_bis,
    a.strasse = i_strasse, a.plz = i_plz, a.ort = i_ort,
    a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = i_user,
    a.status = i_newstatus
where a.lfdnr = i_lfdnr;
commit;

a1_workflow_status(
  i_lfdnr     => i_lfdnr,
  i_oldstatus => x_status_old,
  i_newstatus => i_newstatus);

end;

------------------------------------------------------------------------

procedure update_delete(i_lfdnr in number, i_function in varchar2, i_user in varchar2) is

cursor c_status_old is
  select a.status from sl_a1_antrag_tab a where a.lfdnr = i_lfdnr;

x_user       varchar2(200);
x_status_new varchar2(255);
x_status_old varchar2(255);

begin

x_user := nvl(i_user, 'SIVAS');

open c_status_old;
  fetch c_status_old into x_status_old;
close c_status_old;

if    i_function = '50' then x_status_new := '50 Bescheinigung erhalten';
  update sl_a1_antrag_tab a
  set a.genehmigt_am = trunc(sysdate), a.genehmigt_von = x_user,
      a.status = x_status_new, a.bearbeitet_von = x_user, a.bearbeitet_am = trunc(sysdate)
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '40' then x_status_new := '40 vorläufige Bescheinigung erhalten';
  update sl_a1_antrag_tab a
  set a.vorl_erh_am = trunc(sysdate), a.vorl_erh_von = x_user,
      a.status = x_status_new, a.bearbeitet_von = x_user, a.bearbeitet_am = trunc(sysdate)
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '30' then x_status_new := '30 zur Beantragung an Krankenkasse';
  update sl_a1_antrag_tab a
  set a.beantragt_am = trunc(sysdate), a.beantragt_von = x_user,
      a.status = x_status_new, a.bearbeitet_von = x_user, a.bearbeitet_am = trunc(sysdate)
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '20' then x_status_new := '20 in Bearbeitung Perso';
  update sl_a1_antrag_tab a
  set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '10' then x_status_new := '10 in Arbeit';
  update sl_a1_antrag_tab a
  set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '90' then x_status_new := '90 Antrag ungültig (löschen)';
  update sl_a1_antrag_tab a
  set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '80' then x_status_new := '80 geändert';
  update sl_a1_antrag_tab a
  set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
  where a.lfdnr = i_lfdnr; commit;

elsif i_function = '85' then x_status_new := '85 Stornierung beantragt';
  update sl_a1_antrag_tab a
  set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
  where a.lfdnr = i_lfdnr; commit;

end if;

a1_workflow_status(
  i_lfdnr     => i_lfdnr,
  i_oldstatus => x_status_old,
  i_newstatus => x_status_new);

end;

------------------------------------------------------------------------

procedure a1_workflow_status(i_lfdnr in number, i_oldstatus in varchar2, i_newstatus in varchar2) is

cursor c_antrag is
  select * from sl_a1_antrag_tab a where a.lfdnr = i_lfdnr;

xx_antrag c_antrag%rowtype;

x_pers   number;
x_user   varchar2(100);

cursor c_pde_user is
  select t.e_mail from pde_persstamm t
  where nvl(ausblenden_jn,'N') = 'N'
    and (to_char(austrdat,'yyyy') >= to_char(sysdate,'yyyy') or austrdat is null)
    and (to_char(eintrdat,'YYYY') <= to_char(sysdate,'yyyy'))
    and lower(t.login_user) = lower(x_user);

cursor c_pde_angelegt_von is
  select t.e_mail from pde_persstamm t
  where nvl(ausblenden_jn,'N') = 'N'
    and (to_char(austrdat,'yyyy') >= to_char(sysdate,'yyyy') or austrdat is null)
    and (to_char(eintrdat,'YYYY') <= to_char(sysdate,'yyyy'))
    and lower(t.login_user) = lower(x_user);

x_mail_body  varchar2(1000);
x_send_to    varchar2(100);
x_send_from  varchar2(100);

begin

if i_oldstatus != i_newstatus then

  open c_antrag;
    fetch c_antrag into xx_antrag;
  close c_antrag;

  -- Status 20 / 60 / 80: Antragsteller → pv@servolift.de
  if substr(i_newstatus,1,2) in ('20','60','80') then
    x_user := xx_antrag.angelegt_von;
    open c_pde_user; fetch c_pde_user into x_send_from; close c_pde_user;
    x_send_to   := 'pv@servolift.de';
    x_mail_body :=
      'A1 Antrag angelegt bzw. geändert'          || chr(13)|| chr(13) ||
      'LFDNR: '        || xx_antrag.lfdnr          || chr(13) ||
      'Mitarbeiter: '  || xx_antrag.pers_nr || ', ' || xx_antrag.fam_name || ', ' || xx_antrag.name_vorname || chr(13) ||
      'VON: '          || xx_antrag.von || ', BIS: ' || xx_antrag.bis    || chr(13) ||
      'Firma: '        || xx_antrag.firma || ', Land: ' || xx_antrag.land || chr(13) ||
      'Angelegt am: '  || xx_antrag.angelegt_am || ', von: ' || xx_antrag.angelegt_von || chr(13)|| chr(13) ||
      'Status alt: '   || i_oldstatus || chr(13) || 'Status neu: ' || i_newstatus;
    if x_send_from is not null and x_send_to is not null then
      sendmail_smtp(SendTo => x_send_to, SendFrom => x_send_from,
                    MailSubject => 'A1 Antrag angelegt bzw. geändert',
                    MailBody => x_mail_body, SmtpHost => 'slex');
    end if;
  end if;

  -- Status 85: Stornierung → pv@servolift.de
  if substr(i_newstatus,1,2) = '85' then
    x_user := xx_antrag.angelegt_von;
    open c_pde_user; fetch c_pde_user into x_send_from; close c_pde_user;
    x_send_to   := 'pv@servolift.de';
    x_mail_body :=
      'A1 Antrag - Stornierung von Mitarbeiter beantragt' || chr(13)|| chr(13) ||
      'LFDNR: '        || xx_antrag.lfdnr          || chr(13) ||
      'Mitarbeiter: '  || xx_antrag.pers_nr || ', ' || xx_antrag.fam_name || ', ' || xx_antrag.name_vorname || chr(13) ||
      'VON: '          || xx_antrag.von || ', BIS: ' || xx_antrag.bis    || chr(13) ||
      'Firma: '        || xx_antrag.firma || ', Land: ' || xx_antrag.land || chr(13) ||
      'Angelegt am: '  || xx_antrag.angelegt_am || ', von: ' || xx_antrag.angelegt_von || chr(13)|| chr(13) ||
      'Status alt: '   || i_oldstatus || chr(13) || 'Status neu: ' || i_newstatus;
    if x_send_from is not null and x_send_to is not null then
      sendmail_smtp(SendTo => x_send_to, SendFrom => x_send_from,
                    MailSubject => 'A1 Antrag Stornierung',
                    MailBody => x_mail_body, SmtpHost => 'slex');
    end if;
  end if;

  -- Status 30 / 40 / 50: PZE → Antragsteller
  if substr(i_newstatus,1,2) in ('30','40','50') then
    x_user := xx_antrag.angelegt_von;
    open c_pde_angelegt_von; fetch c_pde_angelegt_von into x_send_to; close c_pde_angelegt_von;
    x_send_from := 'pv@servolift.de';
    x_mail_body :=
      'A1 Antrag Statusänderung'                   || chr(13)|| chr(13) ||
      'LFDNR: '        || xx_antrag.lfdnr          || chr(13) ||
      'Mitarbeiter: '  || xx_antrag.pers_nr || ', ' || xx_antrag.fam_name || ', ' || xx_antrag.name_vorname || chr(13) ||
      'VON: '          || xx_antrag.von || ', BIS: ' || xx_antrag.bis    || chr(13) ||
      'Firma: '        || xx_antrag.firma || ', Land: ' || xx_antrag.land || chr(13) ||
      'Angelegt am: '  || xx_antrag.angelegt_am || ', von: ' || xx_antrag.angelegt_von || chr(13)|| chr(13) ||
      'Status alt: '   || i_oldstatus || chr(13) || 'Status neu: ' || i_newstatus;
    if x_send_from is not null and x_send_to is not null then
      sendmail_smtp(SendTo => x_send_to, SendFrom => x_send_from,
                    MailSubject => 'A1 Antrag Statusänderung',
                    MailBody => x_mail_body, SmtpHost => 'slex');
    end if;
  end if;

end if;

end;

END SL_A1_ANTRAG;
/
