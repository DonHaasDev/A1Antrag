# SL_ALLGEMEIN

- Typ: PACKAGE
- Extrahiert: 2026-06-16 12:53:13
- Quelle: (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.10.36)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=linux)))

```sql
CREATE OR REPLACE EDITIONABLE PACKAGE "SIVAS"."SL_ALLGEMEIN" is

  -- Author  : JUNKER
  -- Created : 23.11.2016 08:43:00
  -- Purpose : 
  
function ermittle_datum(i_datum  in date,
                          i_sprache   in varchar2) return varchar2;
function ermittle_tagestyp(i_datum in date) return number;                          

function ermittle_landbez(i_landcode in varchar2,
                          i_sprache   in varchar2) return varchar2;
function ermittle_landbez_isocode(i_iso_code in varchar2,
                          i_sprache   in varchar2) return varchar2;
function ermittle_tag_kw(i_jahr in varchar2,
                         i_kw in varchar2,
                         i_tag in number) return date;   
function ermittle_tag_ikw(i_jahr in varchar2,
                          i_kw in varchar2,
                          i_tag in number) return date;   -- als Wochentag Montag, Dienstag..                     
function ermittle_status (i_status in number) return varchar2;
function ermittle_absagegrund (i_status in number) return varchar2;                        
function ermittle_db return varchar2;
function sl_squeeze(IN_VAL in varchar2) return varchar2;
procedure sl_user_vorl_kopieren(i_quell_user in varchar2,i_ziel_user in varchar2);
procedure sl_beswunsch_layout_bereinigen(i_ziel_user in varchar2);
procedure sl_truncate_K_T_L;                
procedure vorgang_erstellen(i_ueb_lfdnr in number default null,
                           i_titel in varchar2,
                           i_protokolltyp in varchar2,
                           i_kontaktart in varchar2,
                           i_zustaendiger in varchar2,
                           i_prio in number,
                           i_text in varchar2,
                           i_prodauftr in varchar2, 
                           i_datum_bis in date default null,
                           i_bearbeiter in varchar2 default null);
procedure vorgang_abschliessen(i_lfdnr in number);
function ermittle_diff_at(i_datum1 in date,i_datum2 in date) return number;
function is_workday(i_datum in date) return varchar2;

function split_string(i_text varchar2, i_trennzei in varchar2) return number;
function split_string_2(i_text varchar2, i_trennzei in varchar2) return number;
function bez_abteilung(iabtkz varchar) return varchar2;
procedure ausgeschiedene_ma_ausbl;
procedure fuelle_allocs_ebene(i_anr in varchar2);
function get_kundeninfo(i_gepa_c1 varchar2, i_kdnr number) return varchar2;
function get_rechnungsinfo(i_gepa_c1 varchar2, i_kdnr number) return varchar2;
function get_lieferinfo(i_gepa_c1 varchar2, i_kdnr number) return varchar2;
function get_kundenadresse(i_gepa_c1 varchar2, i_kdnr number) return varchar2;
procedure nachverfolgung_mail;


procedure sl_user_vorlage_kop_ek;
function sl_ermittle_e_mail_adresse(i_tabelle    in varchar2,
                                   i_schluessel in varchar2,
                                   i_protok     in varchar2) return varchar2;
function ermittle_e_mail_betreff(i_tabelle    in varchar2,
                                  i_schluessel in varchar2,
                                  i_sprache in varchar2,
                                  i_protok     in varchar2) return varchar2;
function ermittle_fibu_konto_bez(i_konto in varchar2) return varchar2;
procedure historie_favorit_schreiben(i_tabelle in varchar2,i_schluessel in varchar2);
procedure historie_favorit_loeschen(i_tabelle in varchar2,i_schluessel in varchar2);
function pruefe_abt_kz_zustaendig(i_benutzer in varchar2,
                                    i_persnr   in number,
                                    i_abt_kz   in varchar2) return number;
function pruefe_pers_zustaendig (i_user varchar2) return varchar2;
function ermittle_abt_kz (i_user varchar2) return varchar2;
procedure ermittle_mitarb_zustaendig(i_user in varchar2);
procedure ermittle_mitarb_abteilung(i_user in varchar2);
procedure ermittle_mitarb_alle(i_user in varchar2);
function ermittle_warenstat_ek(i_teilenr in varchar2) return varchar2;

/* Function ermittelt aktuellste Teilenummer bei Eintragung in ersetzt_durch (verknummer) im Teilestamm
Gibt null zuruck wenn i_teilenr keine ersetzt durch hat ansonsten die aktuellste "ersetzt durch" Teilenummer*/
function ermittle_ersetzt_durch_loop(i_teilenr varchar2) return varchar2;
--procedure sl_tab_size;
--------------------------- Ausgangsrechnung-----------------
 function ermittle_rk_netto(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number) return number;
 function ermittle_rk_steuer(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number) return number;
 function ermittle_rp_netto(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number,i_posnr in number) return number;
 function ermittle_rp_steuer(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number,i_posnr in number) return number;
 --------
 
 function ermittle_kopf_anr(i_prodauftr in varchar2) return varchar;
 function ermittle_kopf_status(i_prodauftr in varchar2) return varchar;
 function ermittle_kopf_termin(i_prodauftr in varchar2,i_terminkz in varchar2) return varchar;
 function ermittle_kopf_termin_tag(i_prodauftr in varchar2,i_terminkz in varchar2) return date;
 function ermittle_kst_bez(i_prodauftr in varchar2,i_agnr number) return varchar;
 function ermittle_kst_bez_kpl(i_bde_nr number) return varchar;
 function ermittle_kopf_prodauftr(i_anr in varchar2) return varchar;
 function ermittle_obersten_knoten(i_prodauftr in varchar2) return varchar;
 function ermittle_oberste_bez(i_prodauftr in varchar2) return varchar;
 function ermittle_oberste_teilenr(i_prodauftr in varchar2) return varchar;
 function ermittle_kopf_anr_und_bez(i_prodauftr in varchar2) return varchar;  --"anr - bez"
 function ermittle_projekt_und_kunde(i_prodauftr in varchar2) return varchar; --"Kom - Name1"
 function ermittle_kommision(i_prodauftr in varchar2) return varchar;
 function ermittle_kfm_aus_prodauftr(i_prodauftr in varchar2) return varchar;
 function ermittle_auftrag_aus_renr(i_renr in varchar2) return varchar;      
 function get_vertriebshinweis(i_teilenr varchar2) return varchar2; 
 function ermittle_parts_index(i_teilenr in varchar2) return varchar2;
 function ermittle_allocs_index(i_prodauftr in varchar2,i_posnr in number) return varchar2;   
 function ermittle_wrkord_index(i_prodauftr in varchar2) return varchar2;
 function ermittle_wrk_ind(i_prodauftr in varchar2) return varchar2;
 function ermittle_ende_pep_termin(i_anr varchar2, i_terminart varchar2) return date;     
 function ermittle_notiz(i_schluessel varchar2, i_tabelle varchar2) return varchar;
 function ermittle_saegeteil(i_teilenr in varchar2) return varchar2;  
 function ermittle_blechteil(i_teilenr in varchar2) return varchar2;
 function ermittle_bezug(i_anr in varchar2) return varchar;
 FUNCTION ermittle_merkmal(i_prodauftr     in varchar2,
                          i_posnr         in number,
                          i_leisten_nr    in number) RETURN varchar;
 FUNCTION ermittle_merkmal_aufruf(i_prodauftr     in varchar2,
                              i_posnr         in number,
                              i_leisten_nr    in number) RETURN varchar;
 FUNCTION ermittle_merkmal_kfm(i_a_art         in varchar2,
                              i_jahr               in number,
                              i_anr             in number,
                              i_posnr           in number,
                              i_leisten         in number
                          ) RETURN varchar;                         
 function ermittle_stueckzeit_ist(i_prodauftr in varchar2,i_kostenst in varchar2,i_kenner in varchar2) return number;
 function ermittle_ersatz_fuer(i_teilenr varchar2) return varchar2; 
 function ermittle_lagerkenner(i_teilenr in varchar2) return varchar2; 
 function ermittle_aend_index(i_prodauftr varchar2, i_posnr in number) return varchar2;                          
 function erm_ende_pep_termin_neumontage(i_anr varchar2) return date;  
 function ermittle_hauptlagerort(i_teilenr in varchar2) return varchar2;                       
   -------------- Zeichnung ermitteln----------------------------                      
 function ermittle_tif_allocs(i_prodauftr varchar2, i_posnr in number) return varchar2; 
 function ermittle_tif_wrkord(i_prodauftr varchar2) return varchar2;
 function ermittle_tif_parts(i_teilenr varchar2) return varchar2  ;   
 function ermittle_tif_parts_aend(i_teilenr varchar2,i_aend_index in varchar2) return varchar2;   
 ---------------- BDE---------------
 function ermittle_sammel_ag(i_bde_nr in number) return varchar2;   
 
 --------------SIK-----------
 procedure fuelle_sik(i_lfd_nr in number, 
                      i_schluessel in varchar2, 
                      i_text in varchar2,
                      i_wert in number,
                      i_datum in date);
 function ermittle_sik_text(i_lfd_nr in number,
                            i_schluessel in varchar2) return varchar2;
 function ermittle_sik_zahl(i_lfd_nr in number,
                            i_schluessel in varchar2) return number;
 function ermittle_sik_datum(i_lfd_nr in number,
                            i_schluessel in varchar2) return date; 
 function ermittle_sik_bez(i_lfd_nr in number) return varchar2;
 function ermittle_sik_wert(i_lfd_nr in number,
                            i_schluessel in varchar2) return varchar2; 
 function ermittle_sik_wert_key(i_kenner in varchar2,
                               i_schluessel in varchar2,
                               i_klasse in number,
                               i_leiste in varchar2,
                               i_lfd_nr in number ) return varchar2 ;                                                                                   
 function ermittle_sik_schluessel(i_kenner in varchar2,
                                  i_tabelle in varchar2,
                                  i_schluessel in varchar2 ) return varchar2;
                                                        


 --------------USER/Rollen-----------
function ermittle_user_zu_rolle(i_rolle varchar2) return boolean;


---------------Reklamation-------------
function ermittle_reklamation(i_prodauftr varchar2) return number;

---------------Verlagerung---------------
  function ermittle_verlagerung(i_prodauftr in varchar2,i_posnr in number) return varchar;
  
---------------Preise---------------
function ermittle_vk_preis(i_teilenr varchar2, i_jahre_zurueck varchar2) return number;
function ermittle_grundpreis(i_teilenr varchar2, i_preis_art varchar2 default 'N') return number;
procedure setze_grundpreis(i_teilenr varchar2, 
                           i_grundpreis number, 
                           i_ab_datum date default trunc(sysdate), 
                           i_preis_art varchar2 default 'N', 
                           omeldung out varchar2);
                           
function ermittle_kosten_prodauftr(i_prodauftr varchar2,
                                     i_pos_nr    number
                                     )
    return number;

end SL_ALLGEMEIN;

CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SIVAS"."SL_ALLGEMEIN" is

function ermittle_datum(i_datum     in date,
                          i_sprache   in varchar2) return varchar2 is

g_datum varchar2(20); 

begin

 case i_sprache 
   when 'E' then 
     g_datum := rtrim(to_char(i_datum,'fmDD Month','NLS_DATE_LANGUAGE=english'))||to_char(i_datum,' YYYY');
   when 'F' then 
     --g_datum := rtrim(to_char(i_datum,'dd month','NLS_DATE_LANGUAGE=french'))||to_char(i_datum,' YYYY');
     g_datum := to_number((to_char(i_datum,'dd')))||rtrim(to_char(i_datum,' fmmonth','NLS_DATE_LANGUAGE=french'))||to_char(i_datum,' YYYY');
   when 'I' then 
     g_datum := rtrim(to_char(i_datum,'DD. fmMonth','NLS_DATE_LANGUAGE=italian'))||to_char(i_datum,' YYYY');
   when 'S' then 
     g_datum := rtrim(to_char(i_datum,'DD. fmMonth','NLS_DATE_LANGUAGE=spanish'))||to_char(i_datum,' YYYY');
   else  
     g_datum := to_char(i_datum,'fmDD. Month YYYY','NLS_DATE_LANGUAGE=german');
 end case;   

  return(g_datum);
end;
-----------------------------------
function ermittle_tag_kw(i_jahr in varchar2,i_kw in varchar2,i_tag in number) return date is
  
cursor c_datum(ix_iw in varchar2) is

select datum from PDE_FABKAL t where to_char(t.datum,'YYYYIW') = ix_iw
and lfdtag = i_tag;
 x_datum date;
 x_iw varchar2(10);
 
 begin 
 
 -- 23.06.2021 DH --> erganzt da LT_SL = Jahr 3000 KW 01 
                  --  momentan verwendet wird fur Projekte die unklar sind
 if i_jahr = '3000' then
   return '01.01.3000';
 end if;
 --
   x_iw := i_jahr||trim(to_char(to_number(i_kw),'09'));
 open c_datum(x_iw);
 fetch c_datum into x_datum;
 close c_datum;

  return(x_datum);
  
  end ;
  ----------------------------------------
function ermittle_tag_ikw(i_jahr in varchar2,i_kw in varchar2,i_tag in number) return date is  -- als Wochentag Montag, Dienstag.. 
  

cursor c_datum is
SELECT 
  TRUNC(NEXT_DAY(TO_DATE(i_jahr || '-01-04', 'YYYY-MM-DD') - 7, i_tag) + (i_kw - 1) * 7) 
FROM dual;

x_datum date;
 
 begin 

if nvl(i_jahr,'0') = '0' or nvl(i_kw,'0') = '0'  then
  return null;
else  
 open c_datum;
   fetch c_datum into x_datum;
 close c_datum;

  return(x_datum);
end if;  
  
  end ; 
  --------------------------------------
  function ermittle_tagestyp(i_datum in date) return number is
     
     cursor c_tagestyp is
          select a.tagestyp
          from pde_fabkal a
          where a.datum = trunc(i_datum);
     x_tagestyp number;
          
    begin
      open c_tagestyp;
         fetch c_tagestyp into x_tagestyp;
      close c_tagestyp;
      
      return(x_tagestyp);
     end;
      
--------------------------------------------------------------------------------

function ermittle_landbez(i_landcode in varchar2,
                                i_sprache   in varchar2) return varchar2 is

cursor c_landbez_fremdspr is
   select landbez from conam_sprache
    where landescode = i_landcode
      and sprache = i_sprache;
      
cursor c_landbez is
   select a.landbez from conam a
    where a.landescode = i_landcode;     


x_landbez varchar2(50); 

begin
  
open c_landbez_fremdspr;
     fetch c_landbez_fremdspr into x_landbez;
     
     if c_landbez_fremdspr%notfound then
        open c_landbez;
             fetch c_landbez into x_landbez;
        close c_landbez;
     end if;
 
close c_landbez_fremdspr;

  return(x_landbez);
end;
--------------------------------------------------------------------------------

function ermittle_landbez_isocode(i_iso_code in varchar2,
                                i_sprache   in varchar2) return varchar2 is

cursor c_landbez_fremdspr is
   select a.landbez from conam_sprache a
    where a.landescode = (select b.landescode from conam b where b.iso_code = i_iso_code)
      and sprache = i_sprache;
      
cursor c_landbez is
   select a.landbez from conam a
    where a.iso_code = i_iso_code;     


x_landbez varchar2(50); 

begin
  
open c_landbez_fremdspr;
     fetch c_landbez_fremdspr into x_landbez;
     
     if c_landbez_fremdspr%notfound then
        open c_landbez;
             fetch c_landbez into x_landbez;
        close c_landbez;
     end if;
 
close c_landbez_fremdspr;

  return(x_landbez);
end;
--------------------------------------------------------------------------------------------
-- Ruckgabewert: LINUX oder SCHULNEU
function ermittle_db return varchar2 is
  
  cursor c_db is 
    --select substr(global_name,1,instr(global_name,'.')-1) from SYS.GLOBAL_NAME;
    select upper(instance_name) from v$instance;
    
  x_db varchar2(50);
  
  begin
    
    open c_db;
    fetch c_db into x_db;
    close c_db;
    
    return(x_db);

end;

-----------------------------------------------------------------------------------------------
-- reduziert mehrfach vorkommende Leerzeichen auf max. eines
FUNCTION sl_squeeze(IN_VAL IN VARCHAR2) RETURN VARCHAR2 is
  
     VAR_TEMP VARCHAR2(2000);
    
BEGIN
     VAR_TEMP := TRIM(IN_VAL);
     WHILE INSTR(VAR_TEMP,'  ') > 0 LOOP
          VAR_TEMP := REPLACE(VAR_TEMP,'  ',' ');
     END LOOP;
     
     RETURN VAR_TEMP;
END;
------------------------------------------------------------------------

procedure sl_user_vorlage_kop_ek is
  
cursor c_pers is 
  select a.persnr,a.login_user from sl_pers_zustaendig a
  where a.austritt > sysdate
  and a.login_user is not null
  and a.benutzer = 'GRIESE';
  
  
  begin
    
  sl_pze.sl_ermittle_pers_zustaendig('GRIESE');
  
  for xx_pers in c_pers loop
    
         sl_user_vorl_kopieren(i_quell_user => 'EK_LAYOUT',
                               i_ziel_user => xx_pers.login_user);
                      
  end loop;
    
  
end;
-----------------------------------

-- Benutzervorlagen kopieren
procedure sl_user_vorl_kopieren(i_quell_user in varchar2,i_ziel_user in varchar2) is
  
  begin
    
   delete from sivas_layout a where a.benutzer = i_ziel_user;
   delete from sivas_net_favoriten a where a.benutzer = i_ziel_user;
   delete from sivas_suche_ergebnis_spalten a where a.benutzer = i_ziel_user;
   delete from sivas_funktions_favoriten a where a.anwender = i_ziel_user;
   delete from sivas_net_vm_properties a where a.username = i_ziel_user;
   delete from sivas_net_window_einstellungen a where a.benutzer = i_ziel_user;
    
   insert into sivas_layout(text,benutzer,formsname,layoutname,standard_jn,layoutbez)
           select a.text,i_ziel_user,a.formsname,a.layoutname,a.standard_jn,a.layoutbez 
           from sivas_layout a
   where a.benutzer = i_quell_user;
    
   insert into sivas_net_favoriten(benutzer,id,menue_name,knoten_nr,von_menue_name,reihenfolge)
           select i_ziel_user,b.id,b.menue_name,b.knoten_nr,b.von_menue_name,b.reihenfolge 
           from sivas_net_favoriten b
   where b.benutzer = i_quell_user;
  
   insert into sivas_suche_ergebnis_spalten (owner,tabelle,spalte,breite,lfdnr,sort_lfdnr,sort_art,benutzer)
          select c.owner,c.tabelle,c.spalte,c.breite,c.lfdnr,c.sort_lfdnr,c.sort_art,i_ziel_user
          from sivas_suche_ergebnis_spalten c
          where c.benutzer = i_quell_user;

   insert into sivas_funktions_favoriten(usercontrol,funktion,anwender,erfass_datum,sivas_block_name,typ)
           select d.usercontrol,d.funktion,i_ziel_user,d.erfass_datum,d.sivas_block_name,d.typ 
             from sivas_funktions_favoriten d 
           where  d.anwender = i_quell_user;    
                                    
   insert into sivas_net_vm_properties(viewmodel,property,value,username,rf)
           select e.viewmodel,e.property,e.value,i_ziel_user,e.rf 
             from sivas_net_vm_properties e
           where e.username = i_quell_user;
           
   insert into sivas_net_window_einstellungen(benutzer,usercontrol,name,hoehe,breite,xpos,ypos,pinned)
          select i_ziel_user,f.usercontrol,f.name,f.hoehe,f.breite,f.xpos,f.ypos,f.pinned
          from sivas_net_window_einstellungen f
          where f.benutzer = i_quell_user ;     
   --

   end;
 -----------------------------------------
 --Fehler im Bestellwunschlayout beheben
 procedure sl_beswunsch_layout_bereinigen(i_ziel_user in varchar2) is
  
  begin
    
   
delete from sivas_net_vm_properties d
where d.username = upper(i_ziel_user)
           and d.viewmodel like 'Bestellwu%';
                                    
   insert into sivas_net_vm_properties(viewmodel,property,value,username,rf)
           select e.viewmodel,e.property,e.value,upper(i_ziel_user),e.rf 
             from sivas_net_vm_properties e
           where e.username = 'EK_LAYOUT'
           and e.viewmodel like 'Bestellwu%';
    end;       
---------------------------------------------- 
-- truncate table PDB.K_T_LOGGING 
procedure sl_truncate_K_T_L is
  
  begin
    
  execute immediate 'truncate table pdb.k_t_logging';
  --delete from pdb.k_t_logging; 


   end;
   
--------------------------------------------------------------------------------------------

procedure vorgang_erstellen(i_ueb_lfdnr in number default null,
                           i_titel in varchar2,
                           i_protokolltyp in varchar2,
                           i_kontaktart in varchar2,
                           i_zustaendiger in varchar2,
                           i_prio in number,
                           i_text in varchar2,
                           i_prodauftr in varchar2, 
                           i_datum_bis in date default null,
                           i_bearbeiter in varchar2 default null) is

x_lfdnr number;

x_text varchar2(4000);

begin
  x_lfdnr := projekt_protokoll_pck.erstellen(i_lfd_nr        => null,
                                             i_lfd_nr_ueb    => i_ueb_lfdnr,
                                             i_kdnr          => null,
                                             i_gepa_c1       => null,
                                             i_partner_nr    => null,
                                             i_datum         => sysdate,
                                             i_objekt_typ    => 'TECH_AUFTRAG',
                                             i_a_art         => null,
                                             i_jahr          => null,
                                             i_anr           => null,
                                             i_projekt_lfdnr => null,
                                             i_posnr         => null,
                                             i_unter_posnr   => null,
                                             i_prodauftr     => i_prodauftr,
                                             i_teilenr       => null,
                                             i_persnr        => null);

  /*x_text := auf_texte_pck.text_konvertieren(i_text => i_text,
                                            i_ueb => null,
                                            i_text_fett_jn => null,
                                            i_text_kursiv_jn => null,
                                            i_text_unterstrichen_jn => null,
                                            i_ueb_fett_jn => null,
                                            i_ueb_kursiv_jn => null,
                                            i_ueb_unterstrichen_jn => null,
                                            i_protok => 'N');*/

  update PROJEKT_PROTOKOLL set text = i_titel,
                               protokolltyp = i_protokolltyp,
                               kontakt_art = i_kontaktart,
                               wichtigkeit = i_prio,
                               vorgangstext = i_text, --x_text,
                               kurzz_zustaendigkeit = i_zustaendiger, 
                               wiedervorlage_datum_ende = i_datum_bis,
                               wiedervorlage_datum = i_datum_bis,
                               kurzz_bearbeiter = i_bearbeiter
  where lfdnr = x_lfdnr;

  --return x_lfdnr;
         
end;

--------------------------------------------------------------------------------------------

procedure vorgang_abschliessen(i_lfdnr in number) is
  
begin
  update PROJEKT_PROTOKOLL 
     set erledigt_nj = 'J',
         erledigt_durch = user,
         erledigt_am = sysdate
   where lfdnr = i_lfdnr;

end;
-----------------------------------
function ermittle_diff_at(i_datum1 in date,i_datum2 in date) return number is
  
cursor c_datum1 is
select t.arbeitstag,to_number(to_char(t.datum,'YYYY')) from PDE_FABKAL t where t.datum = trunc(i_datum1);
 x_tag1 number;
 x_jahr1 number;
 
cursor c_max_datum1 is
  select t.arbeitstag from pde_fabkal t 
  where t.datum = (select  max(s.datum) from pde_fabkal s
  where s.datum < i_datum1
  and s.arbeitstag is not null);

cursor c_datum2 is
select t.arbeitstag,to_number(to_char(t.datum,'YYYY')) from PDE_FABKAL t where t.datum = trunc(i_datum2);
 x_tag2 number;
 x_jahr2 number;
 
cursor c_max_datum2 is
  select t.arbeitstag from pde_fabkal t 
  where t.datum = (select  max(s.datum) from pde_fabkal s
  where s.datum < i_datum2
  and s.arbeitstag is not null); 
 
cursor c_max_at is
select max(t.arbeitstag) from pde_fabkal t where to_char(t.datum,'YYYY') = to_char(i_datum1,'YYYY');
x_max_at number;

x_tage number;


 
 begin 
 open c_datum1;
    fetch c_datum1 into x_tag1,x_jahr1;
 close c_datum1;
 
 if x_tag1 is null then
   open c_max_datum1;
       fetch c_max_datum1 into x_tag1;
   close c_max_datum1;
 end if;      
 
  open c_datum2;
    fetch c_datum2 into x_tag2,x_jahr2;
 close c_datum2;
 
  if x_tag2 is null then
   open c_max_datum2;
       fetch c_max_datum2 into x_tag2;
   close c_max_datum2;
 end if; 
 
 open c_max_at;
      fetch c_max_at into x_max_at;
 close c_max_at;   
 
 if x_jahr1 = x_jahr2 then
   x_tage := x_tag2-x_tag1;
 end if;
 if x_jahr1 = x_jahr2-1 then
   x_tage := x_tag2+(x_max_at-x_tag1);
 end if;      

  return(x_tage);
  end ;
--------------------------------------------------------------------------------

function is_workday(i_datum in date) return varchar2 is
  
  cursor c1 is
    select 'J' from centre_fabkal a
     where a.datum = trunc(i_datum)
       and a.arbeitstag is not null;
x_erg varchar2(1);

begin
  x_erg:='N';  

  open c1;
  fetch c1 into x_erg;
  close c1;
  
  return x_erg;

end;
  
procedure set_packliste_jn(i_prodauftr in varchar2, i_posnr in number) is

  cursor c_pack_jn is
    select a.kontliscode from allocs a
     where a.prodauftr = i_prodauftr
       and a.posnr = i_posnr;  
  
  x_pack_jn varchar2(1);
  
begin
  
  open c_pack_jn;
  fetch c_pack_jn into x_pack_jn;
  close c_pack_jn;
  
  if x_pack_jn = 'J' then
    update allocs a set a.kontliscode = 'N' where a.prodauftr = i_prodauftr and a.posnr = i_posnr;
  else
    update allocs a set a.kontliscode = 'J' where a.prodauftr = i_prodauftr and a.posnr = i_posnr;
  end if;
  
end;

------------------------------------------------------

function split_string(i_text varchar2, i_trennzei in varchar2) return number is

cursor c_count is
select count(regexp_substr(i_text,'(.*?)( |$)', 1, level, null, 1)) element from dual
connect by level <= regexp_count(i_text, i_trennzei)+1;  

x_count number(11);

cursor c_split is
select regexp_substr(i_text,'(.*?)( |$)', 1, level, null, 1) element from dual
connect by level <= regexp_count(i_text, i_trennzei)+1;  

x_split varchar2(255);

begin

delete from sl_split_string;
commit;

open c_count;
  fetch c_count into x_count;
close c_count;

if x_count > 0 then
  open c_split;
    loop
      fetch c_split into x_split;
      exit when c_split%notfound;
           if x_split != i_trennzei then
             insert into sl_split_string(text) values (x_split);
             commit;
           end if;
    end loop;
  close c_split;
end if;
return x_count;

end;

------------------------------------------

function split_string_2(i_text varchar2, i_trennzei in varchar2) return number is

cursor c_count is
select nvl(count(regexp_substr(i_text,'[^' || i_trennzei || ']+', 1, level)),0) from dual
connect by regexp_substr(i_text,'[^' || i_trennzei || ']+', 1, level) is not null; 

x_count number(11);

cursor c_split is
select regexp_substr(i_text,'[^' || i_trennzei || ']+', 1, level) from dual
connect by regexp_substr(i_text,'[^' || i_trennzei || ']+', 1, level) is not null; 

x_split varchar2(255);

begin

delete from sl_split_string;

open c_count;
  fetch c_count into x_count;
close c_count;

if x_count > 0 then
  open c_split;
    loop
      fetch c_split into x_split;
      exit when c_split%notfound;
           if x_split != i_trennzei then
             insert into sl_split_string(text) values (x_split);
           end if;
    end loop;
  close c_split;
end if;
return x_count;

end;

------------------------------------------------------



------------------------------------

  function bez_abteilung(iabtkz varchar) return varchar2 is
    xbez varchar2(2000);
  
    cursor c_abt is
      select a.bezeichnung
        from abteilung a
       where a.abt_kz = iabtkz;
  
  begin
    if iabtkz is null then
      return null;
    end if;
    open c_abt;
    fetch c_abt
      into xbez;
    if c_abt%NOTFOUND then
      xbez := 'ungultige Abteilung';
    end if;
    close c_abt;
    return(xbez);
  end bez_abteilung;
  ---------------------------------------
  -- Blendet Mitarbeiter aus, die vor mehr als 6 Wochen ausgeschieden sind
  -- wird durch Timer 275 "Nachts" ausgefuhrt
  procedure ausgeschiedene_ma_ausbl is
    begin
      update pde_persstamm p
         set p.ausblenden_jn = 'J'
       where nvl(p.ausblenden_jn,'N') = 'N'
         and nvl(p.austrdat, sysdate+1) < sysdate -(6*7);
       update zeiterf_stamm z
          set z.legic_key = Null, z.info2 = Null
        where z.legic_key is not null 
       and z.pers_nummer in
              (select a.persnr
                 from pde_persstamm a
                where nvl(a.austrdat, sysdate + 10) < sysdate - 2);
         
    
  end ausgeschiedene_ma_ausbl;
  ---------------------------------------
  
  procedure fuelle_allocs_ebene(i_anr in varchar2) is
    x_prodauftr varchar2(100);
    
    cursor c_p is 
      select a.prodauftr
        from wrkord a
       where a.a_art in ('TE', 'MZ')
         and a.anr = i_anr;  
  
    begin
      
      open c_p;
      fetch c_p into x_prodauftr;
      close c_p;
      
      delete from allocs_struct;
      allocs_ebene(x_prodauftr); 
    
  end;
  
  ---------------------------------------
  ---------------------------------------
  function get_kundeninfo(i_gepa_c1 varchar2, i_kdnr number) return varchar2 is
  
  x_text varchar2(2000);
  
  cursor c_text is  
    select a.text from sik_k_gepa_kundenkopf a
     where lfd_nr = 193
       and gepa_c1 = i_gepa_c1
       and kdnr = i_kdnr;
   
  begin
    open c_text;
    fetch c_text into x_text;
    close c_text;
    
    return x_text;
  end get_kundeninfo;
  -------------------------------
    function get_rechnungsinfo(i_gepa_c1 varchar2, i_kdnr number) return varchar2 is
  
  x_text varchar2(2000);
  
  cursor c_text is  
    select a.text from sik_k_gepa_kundenkopf a
     where lfd_nr = 1068
       and gepa_c1 = i_gepa_c1
       and kdnr = i_kdnr;
   
  begin
    open c_text;
    fetch c_text into x_text;
    close c_text;
    
    return x_text;
  end get_rechnungsinfo;
  ---------------------------------------
  function get_lieferinfo(i_gepa_c1 varchar2, i_kdnr number) return varchar2 is
  
  x_text varchar2(2000);
  
  cursor c_text is  
    select a.text from sik_k_gepa_kundenkopf a
     where lfd_nr = 192
       and gepa_c1 = i_gepa_c1
       and kdnr = i_kdnr;
   
  begin
    open c_text;
    fetch c_text into x_text;
    close c_text;
    
    return x_text;
  end get_lieferinfo;
  ---------------------------------------
  function get_vertriebshinweis(i_teilenr varchar2) return varchar2 is
  
  x_text varchar2(2000);
  
  cursor c_text is  
    select a.text from sik_k_parts_vkinfo a
     where lfd_nr = 179
       and teilenr = i_teilenr;
   
  begin
    open c_text;
    fetch c_text into x_text;
    close c_text;
    
    x_text := 'Achtung !!!!!'||chr(10)||chr(10)||x_text||chr(10)||chr(10)||'Bitte beachten.';
    
    return x_text;
  end get_vertriebshinweis;
-------------------------------------------
  function get_kundenadresse(i_gepa_c1 varchar2, i_kdnr number) return varchar2 is
  
  x_text varchar2(2000);
  
  cursor c_text is  
    select g.name1 ||chr(13)||g.name2||chr(13)||g.ort||chr(13)||g.land
      from gepa g
     where gepa_c1 = i_gepa_c1
       and kdnr = i_kdnr;
   
  begin
    open c_text;
    fetch c_text into x_text;
    close c_text;
    
    return x_text;
  end get_kundenadresse;  
  --------------------------------------------
  -- Erinnerungs-Email fur Angebots-Nachverfolgung 
  -- Wird durch WF 346 Ausgefuhrt
  procedure nachverfolgung_mail is 
  
  x_text varchar2(2000);  
  
    cursor c_na is
      select a.lfdnr vorg_lfdnr,
             a.datum vorg_datum,
             a.text  vorg_text,
             a.kurzz_zustaendigkeit vorg_zustaendiger,
             replace(replace(replace(replace(replace(a.kurzz_bearbeiter,' '), ',GRP_VK_I'), 'GRP_VK_I,'), ',GRP_VK'), 'GRP_VK,') vorg_bearbeiter,
             trunc(a.erinnerungs_datum) vorg_erinnerungs_datum,
             p.a_art vorg_proj_a_art,
             p.anr   vorg_proj_anr,
             p.posnr vorg_proj_posnr,
             (select bezeichnung from PROJEKT_PROTOKOLL_TYP_STAT where typ = a.protokolltyp and status = a.status) vorg_status,
             p.teilenr           vorg_teilenr,
             k.a_art             vorg_a_art,
             k.anr               vorg_anr,              
             k.name1             vorg_kunde,
             k.land              vorg_land,
             To_Char(SysDate - y.Datum_letzte_Aktion, '9999') vorg_tage_seit_Letzter_Aktion, 
             (select n.text from projekt_protokoll_notizen n where n.vorgang_lfd_nr = a.lfdnr and n.lfdnr = (select max(lfdnr) from projekt_protokoll_notizen where vorgang_lfd_nr = a.lfdnr)) vorg_letzte_notiz
        from projekt_protokoll a,
             projekt_protokoll_objekt_zuo b,
             projekt_pos p,
             auf_kopf k,
             (Select x.ANR, Max(x.Datum) As Datum_letzte_Aktion
                From (Select To_Char(a.ANR) As ANR,
                             NVL(e.AEND_DATUM, a.AUFTRAG_DATUM) As Datum,
                             'Datum_Angebot' As Typ
                        From AUF_KOPF a, EDM_DOK e, EDM_ZUORDNUNG g
                       Where a.SCHLUESSEL = g.SCHLUESSEL(+)
                         And g.LFD_NR = e.LFD_NR(+)
                         And a.A_ART In ('MO', 'OF')
                         And g.TABELLE(+) = 'AUF_KOPF'
                         And e.NEUESTER_STAND_JN(+) = 'J'
                         And e.AEND_BESCHREIBUNG(+) = 'automatisch durch Druck'
                         And e.AUSGANGS_DOKUMENT_JN(+) = 'J'
                      Union
                      Select t.PRO_PRNR As anr,
                             (Select Max(i.DATUM)
                                From SL_TERMINAENDERUNGEN i
                               Where i.PRO_NR = t.PRO_PRNR) As Datum,
                             'Datum_Letzter_PDB_Eintrag' As Typ
                        From SL_PROJEKT t
                       Where t.PRO_STATUS In ('Q', 'O')
                      Union
                      Select t.PRO_PRNR As anr,
                             (Select Max(g.AENDERUNGS_DATUM)
                                From PROJEKT_PROTOKOLL            g,
                                     PROJEKT_PROTOKOLL_OBJEKT_ZUO z
                               Where z.LFDNR = g.LFDNR
                                 And z.PROJEKT_LFDNR = p.LFD_NR) As Datum_Letzter_Vorgang,
                             'Datum_Letzter_Vorgang' As Typ
                        From SL_PROJEKT t, AUF_KOPF a, PROJEKT_POS p
                       Where a.PROJEKT_LFD_NR = p.LFD_NR(+)
                         And t.PRO_PRNR = To_Char(a.ANR)
                         And a.A_ART In ('MO', 'OF')
                         And a.JAHR > 1999) x
               Group By x.ANR) y 
       where a.lfdnr = b.lfdnr
         and nvl(a.erledigt_nj, 'N') = 'N'
         and a.kontakt_art = 'Kundenkontakt'
         and a.protokolltyp = 'Nachfass'
         and nvl(a.erinnern_jn, 'J') = 'J'
         and trunc(a.erinnerungs_datum) = trunc(sysdate)
         --and trunc(a.erinnerungs_datum) = '02.12.2019'
         and b.projekt_lfdnr = p.lfd_nr
         and p.lfd_nr = k.projekt_lfd_nr
         and k.a_art in ('MO')
         and y.anr = to_char(k.anr);
         
    xx_na c_na%rowtype;
    
    cursor c_mail (i_user varchar2) is
      select a.e_mail
        from pde_persstamm a 
       where upper(a.login_user) = upper(i_user);
    
    x_mail varchar2(200);
    
    cursor c_empf (i_empf varchar2) is
      select regexp_substr(i_empf,'[^,]+', 1, level) empfaenger from dual
      connect by regexp_substr(i_empf,'[^,]+', 1, level) is not null;
    
    xx_empf c_empf%rowtype;

  begin
    
    for xx_na in c_na loop
      
      x_text := '<html><body>Vorgang: <a href="sivas: program=ProjektProtokoll instance=servolinux LFDNR=' || xx_na.vorg_lfdnr || '">' || xx_na.vorg_lfdnr || '</a>'
             || '<br>Projekt: ' || xx_na.vorg_proj_anr
             || '<br>Angebot: ' || xx_na.vorg_anr
             || '<br>Kunde: ' || xx_na.vorg_kunde || ' - ' || xx_na.vorg_land
             || '<br>Tage seit letzter Aktion: ' || xx_na.vorg_tage_seit_letzter_aktion
             || '<br>Letzte Notiz: ' || xx_na.vorg_letzte_notiz
             || '<br>VK: ' || xx_na.vorg_zustaendiger
             || '<br>VKI: ' || xx_na.vorg_bearbeiter
             || '</body></html>';
           
      x_mail := null;
      
      if xx_na.vorg_status = 'Nachgefragt' or xx_na.vorg_status is null then -- Mail an Bearbeiter (VI)
        for xx_empf in c_empf(xx_na.vorg_bearbeiter) loop --Schleife durch alle Empfanger, falls mehrere eingetragen sind
          open c_mail(xx_empf.empfaenger);  --Email-Adresse ermitteln
          fetch c_mail into x_mail;
          close c_mail;
          
          sendmail_html_smtp(SendTo => x_mail, --'saelinger@servolift.de'
                    SendFrom => 'sivas@servolift.de', 
                    Mail_Subject => 'Angebot Nachfassen (WF346)', 
                    Mail_HtmlBody => x_text,
                    Mail_TextBody => '',
                    SmtpHost => 'slex',--xx_pv_fm.smtp_server,
                    SmtpPort => 25);
        end loop;
      else -- Status = Ruhend => Mail an Zustandiger (VK)
        for xx_empf in c_empf(xx_na.vorg_zustaendiger) loop --Schleife durch alle Empfanger, falls mehrere eingetragen sind
          open c_mail(xx_empf.empfaenger);  --Email-Adresse ermitteln
          fetch c_mail into x_mail;
          close c_mail;
          
          sendmail_html_smtp(SendTo => x_mail,  --'saelinger@servolift.de',
                    SendFrom => 'sivas@servolift.de', 
                    Mail_Subject => 'Angebot Nachfassen (WF346)', 
                    Mail_HtmlBody => x_text,
                    Mail_TextBody => '',
                    SmtpHost => 'slex',--xx_pv_fm.smtp_server,
                    SmtpPort => 25);
        end loop;
      end if;
                  
      
    end loop;
    
  end nachverfolgung_mail;
  
  ----------------------------------------------------
  -- Funktion aus Allgemein.... kopiert + fehler behoben
  function pruefe_abt_kz_zustaendig(i_benutzer in varchar2,
                                    i_persnr   in number,
                                    i_abt_kz   in varchar2) return number is
  
    cursor c_abt_kz_zustaendig is
      select replace(abt_kz_zustaendig, ', ', ','),
             replace(pers_kz_zustaendig, ', ', ',')
        from pde_persstamm
       where login_user = i_benutzer;
    x_abt_kz_zustaendig  pde_persstamm.abt_kz_zustaendig%type;
    x_pers_kz_zustaendig pde_persstamm.pers_kz_zustaendig%type;
  
    cursor c_persnr is
      select 'J'
        from pde_persstamm
       where ((i_persnr is not null and persnr = i_persnr) or
             i_abt_kz is not null and nvl(abt_kz, 'X?1?X') = i_abt_kz)
         and (instr(',' || nvl(x_abt_kz_zustaendig, 'X?1?X') || ',',
                    ',' || nvl(abt_kz, 'X?2?X') || ',') > 0 or
             (instr(',' || nvl(x_pers_kz_zustaendig, 'X?1?X') || ',',
                     ',' || persnr || ',') > 0));
                     
    cursor c_darf_alles_sehen is 
     select 'J' from dba_role_privs a
      where a.GRANTEE = i_benutzer
      and a.GRANTED_ROLE = 'GRP_PERS';
      x_darf_alles_sehen varchar2(1);
  
    x_persnr varchar2(1);
  
  begin
    
   open c_darf_alles_sehen;
     fetch c_darf_alles_sehen into x_darf_alles_sehen;
     if c_darf_alles_sehen%found then
   -- if i_benutzer in ('JR','SCHWARZ','SIVAS','GOETZ','BRAUN','KOBLISCHEK','BERNHART','SCHMIDT') then 
      return(1);
    end if;
   close c_darf_alles_sehen;
   
    if i_persnr is null and i_abt_kz is null then
      return(0);
    end if;
  
    open c_abt_kz_zustaendig;
    fetch c_abt_kz_zustaendig
      into x_abt_kz_zustaendig, x_pers_kz_zustaendig;
    close c_abt_kz_zustaendig;
  
    if x_abt_kz_zustaendig is not null or x_pers_kz_zustaendig is not null then
    
      open c_persnr;
      fetch c_persnr
        into x_persnr;
      if c_persnr%found then
        close c_persnr;
        return(1);
      end if;
      close c_persnr;
    
      return(0);
    
    end if;
  
    return(0);
  
  exception
    when others then
      sivas_exception('Fehler in allgemein.pruefe_abt_kz_zustaendig! Benutzer: ' ||
                      i_benutzer || ', Person: ' || i_persnr ||
                      ', Abteilung: ' || i_abt_kz || chr(10) || sqlerrm);
      return(0);
    
  end pruefe_abt_kz_zustaendig;

  ----------------------------------------------------------------------------
function pruefe_pers_zustaendig (i_user varchar2) return varchar2 is
 
 x_pers_zus varchar2 (100); 
 x_anz number; 
 
cursor c_anz is
SELECT count(*)
  FROM PDE_PERSSTAMM p
 where allgemein.pruefe_abt_kz_zustaendig(upper(i_user), p.persnr, null) = 1
 and nvl(p.austrdat,sysdate+1) > sysdate-1;
        
 begin 

      open c_anz;
           fetch c_anz into x_anz;
      close c_anz;
      if x_anz > 1 then
        x_pers_zus := 'J'; 
      else 
        x_pers_zus := 'N';
      end if;  
          
  return x_pers_zus;
  exception 
    when others then 
      return ('Fehler');
 end;
   ----------------------------------------------------------------------------
  procedure ermittle_mitarb_zustaendig(i_user in varchar2) is
  
    cursor c_zus is
      SELECT p.persnr,
             p.login_user,
             p.familienname || ', ' || p.vorname Mitarbeiter,
             p.abt_kz,
             'Z' Herkunft,
             p.pze_stemp_aend_verhindern_jn aendern_gesperrt
        FROM PDE_PERSSTAMM p
       where allgemein.pruefe_abt_kz_zustaendig(i_user, p.persnr, null) = 1
         and nvl(p.austrdat, sysdate + 1) > sysdate - 1;
  
    x_zustaendig       varchar2(1) ;
    x_aendern_erlaubt varchar2(1) ;
  
  begin
  
    delete from sl_mitarbeiter_zustaendig a where a.benutzer = i_user;
  
    for xx_zus in c_zus loop
    
           x_aendern_erlaubt := '3';
    
      if xx_zus.login_user = i_user then
        x_zustaendig := sl_allgemein.pruefe_pers_zustaendig(i_user);
        if x_zustaendig = 'J' then 
           x_aendern_erlaubt := '3';
        else 
          x_aendern_erlaubt := '2';
        end if; 
      else 
         x_aendern_erlaubt := '3';    
      end if;
      
    
      insert into sl_mitarbeiter_zustaendig
        (persnr,
         mitarbeiter,
         abt_kz,
         herkunft,
         zustaendig,
         aendern_erlaubt,
         benutzer)
      values
        (xx_zus.persnr,
         xx_zus.mitarbeiter,
         xx_zus.abt_kz,
         'Z',
         x_zustaendig,
         x_aendern_erlaubt,
         i_user);
    end loop;
  
  end;
  ----------------------------------------------------------------------------
  procedure ermittle_mitarb_abteilung(i_user in varchar2) is
  
    cursor c_abt is
      SELECT p.persnr,
             p.login_user,
             p.familienname || ', ' || p.vorname Mitarbeiter,
             p.abt_kz,
             'A' Herkunft,
             p.pze_stemp_aend_verhindern_jn aendern_gesperrt
        FROM PDE_PERSSTAMM p
       where p.abt_kz = (select a.abt_kz from pde_persstamm a where a.login_user = i_user)
         and nvl(p.austrdat, sysdate + 1) > sysdate - 1;
  
    x_zustaendig       varchar2(1) ;
    x_aendern_erlaubt varchar2(1) ;
  
  begin
  
    delete from sl_mitarbeiter_abteilung a where a.benutzer = i_user;
  
    for xx_abt in c_abt loop
    
      x_zustaendig := null;
      x_aendern_erlaubt := 'N';
    
/*      if xx_zus.login_user = i_user then
        x_zustaendig := 'J';
        if xx_zus.aendern_gesperrt = 'J' then
          x_aendern_gesperrt := 'J';
        end if;
      end if;*/
      
      if xx_abt.login_user = i_user then 
          if xx_abt.aendern_gesperrt = 'J' then 
              x_aendern_erlaubt := 2;
           else 
             x_aendern_erlaubt := 3;   
           end if;  
       else        
             x_aendern_erlaubt := 1;
       end if;
             
      insert into sl_mitarbeiter_abteilung
        (persnr,
         mitarbeiter,
         abt_kz,
         herkunft,
         zustaendig,
         aendern_erlaubt,
         benutzer)
      values
        (xx_abt.persnr,
         xx_abt.mitarbeiter,
         xx_abt.abt_kz,
         'A',
         x_zustaendig,
         x_aendern_erlaubt,
         i_user);
    end loop;
  
  end;
  ---------------------
    procedure ermittle_mitarb_alle(i_user in varchar2) is
  
  cursor c_mit is 
      select a.persnr,a.mitarbeiter,a.abt_kz,a.aendern_erlaubt,a.herkunft
           from sl_mitarbeiter_zustaendig a
           where a.benutzer = i_user
           union 
           select b.persnr,b.mitarbeiter,b.abt_kz,b.aendern_erlaubt,b.herkunft
           from sl_mitarbeiter_abteilung b
           where b.benutzer = i_user
           and b.persnr not in (select c.persnr from sl_mitarbeiter_zustaendig c
                                where c.benutzer = i_user) ;
  
  
  
  cursor c_boehler is 
         select a.persnr,a.mitarbeiter,a.abt_kz,a.aendern_erlaubt,a.herkunft
         from sl_mitarbeiter_zustaendig a
         where a.benutzer = 'BOEHLER';
         
  cursor c_lahl is
         select a.persnr,a.mitarbeiter,a.abt_kz,a.aendern_erlaubt,a.herkunft
           from sl_mitarbeiter_zustaendig a
           where a.benutzer = 'LAHL';
           
           
  --Cursor liest aus ob ein Nutzer, Sehberechtigungen auf andere Abteilungen hat.
  cursor c_ABT_sehen_lesen (i_benutzer varchar2) is 
         Select x.text 
         from klas_auspraegung  x
         where x.lfd_nr = 1477
           and x.schluessel = (select a.persnr from pde_persstamm a
                               where a.login_user = i_benutzer);
         
         x_zusaetzliche_Abt_sehen varchar2(4000);
  
  --Cursor liest aus ob ein Nutzer, Schreibrechte auf andere Abteilungen hat.
  cursor c_ABT_schreiben (i_benutzer varchar2) is 
         Select x.text 
         from klas_auspraegung  x
         where x.lfd_nr = 1478
           and x.schluessel = (select a.persnr from pde_persstamm a
                               where a.login_user = i_benutzer);
         
         x_zusaetzliche_Abt_schreiben varchar2(4000);
         
   cursor c_split_string is
          select s.text from sl_split_string s;
               
  x_split_count number;
  --Cursor gibt die entsprechenden Abteilungen weiter und deren Nutzer ein.                              
  cursor c_ABT_sehen_einf(ic_abteilungen varchar2) is               
         select distinct(a.persnr) ,a.mitarbeiter,a.abt_kz,a.aendern_erlaubt,a.herkunft
         from sl_mitarbeiter_zustaendig a
         where a.abt_kz in (ic_abteilungen)
         and a.aendern_erlaubt = 3;         
         
  --Cursor gibt die entsprechenden Abteilungen weiter und deren Nutzer ein.
  cursor c_ABT_schreiben_einf(ic_abteilungen varchar2) is               
         select distinct(a.persnr) ,a.mitarbeiter,a.abt_kz,a.aendern_erlaubt,a.herkunft
         from sl_mitarbeiter_zustaendig a
         where a.abt_kz in (select * from sl_split_string)
         and a.aendern_erlaubt = 3;                     
                               
           
  begin
  
    delete from sl_mitarbeiter_alle a where a.benutzer = i_user;
    
        
    sl_allgemein.ermittle_mitarb_abteilung(i_user);
    sl_allgemein.ermittle_mitarb_zustaendig(i_user);
    
    x_zusaetzliche_Abt_sehen := '';
    x_zusaetzliche_Abt_schreiben := '';
 
    for xx_mit in c_mit loop
          
      insert into sl_mitarbeiter_alle
        (persnr,
         mitarbeiter,
         abt_kz,
         herkunft,
         aendern_erlaubt,
         benutzer)
      values
        (xx_mit.persnr,
         xx_mit.mitarbeiter,
         xx_mit.abt_kz,
         xx_mit.herkunft,
         xx_mit.aendern_erlaubt,
         i_user);
    end loop;
    --Einfugen der zusatzlichen Abteilungen, welche nur gelesen werden durfen.
    open c_ABT_sehen_lesen(i_user);
         fetch c_ABT_sehen_lesen into x_zusaetzliche_Abt_sehen;
         if  c_ABT_sehen_lesen%found then
             
             x_split_count := sl_allgemein.split_string_2(x_zusaetzliche_Abt_sehen, ',');  
             
             for xx_split_string in c_split_string loop
                 for xx_ABT_sehen_einf in c_ABT_sehen_einf(xx_split_string.text) loop
                 insert into sl_mitarbeiter_alle
                        (persnr,
                         mitarbeiter,
                         abt_kz,
                         --herkunft,
                         aendern_erlaubt,
                         benutzer)
                 values
                        (xx_ABT_sehen_einf.persnr,
                         xx_ABT_sehen_einf.mitarbeiter,
                         xx_ABT_sehen_einf.abt_kz,
                         --xx_mit.herkunft,
                         1,
                         i_user);
                end loop;
            end loop;
         end if;
    close c_ABT_sehen_lesen;
    --Einfugen der zusatzlichen Abteilungen die geschrieben werden durfen. 
    open c_ABT_schreiben(i_user);
         fetch c_ABT_schreiben into x_zusaetzliche_Abt_schreiben;
         if c_ABT_schreiben%found then
            x_split_count := sl_allgemein.split_string_2(x_zusaetzliche_Abt_schreiben, ',');  
             
            for xx_split_string in c_split_string loop
                for xx_ABT_schreiben in c_ABT_schreiben_einf(xx_split_string.text) loop
                    insert into sl_mitarbeiter_alle
                            (persnr,
                             mitarbeiter,
                             abt_kz,
                             --herkunft,
                             aendern_erlaubt,
                             benutzer)
                     values
                            (xx_ABT_schreiben.persnr,
                             xx_ABT_schreiben.mitarbeiter,
                             xx_ABT_schreiben.abt_kz,
                             --xx_mit.herkunft,
                             3,
                             i_user);
                    end loop;
                end loop;
         end if;    
     close c_ABT_schreiben;
   
    
    
   --Hier startet eine Sonderbedingung fur den Abteilunsgkalender fur Laurence und Bianca. 
 if i_user = 'LAHL' then
    for xx_boehler in c_boehler loop
        insert into sl_mitarbeiter_alle
        (persnr,
         mitarbeiter,
         abt_kz,
         --herkunft,
         aendern_erlaubt,
         benutzer)
      values
        (xx_boehler.persnr,
         xx_boehler.mitarbeiter,
         xx_boehler.abt_kz,
         --xx_mit.herkunft,
         1,
         i_user);
    end loop;
  elsif i_user = 'BOEHLER' then
        for xx_lahl in c_lahl loop
             insert into sl_mitarbeiter_alle
        (persnr,
         mitarbeiter,
         abt_kz,
         --herkunft,
         aendern_erlaubt,
         benutzer)
      values
        (xx_lahl.persnr,
         xx_lahl.mitarbeiter,
         xx_lahl.abt_kz,
         --xx_mit.herkunft,
         1,
         i_user);
         end loop;
  
                     
  end if;
  --Ende der Sonderbedinung fur Laurence und Bianca. 
  end;

  -----------------------
  --tabellengro?e ermitteln
  /*
  procedure sl_tab_size is
    cursor c_tabname is
      select table_name
          from dba_tables
          where owner = 'SIVAS';
      x_tabname varchar2(2000);
          
     cursor c_tab_count(i_tabname in varchar2) is
       select count(*) from i_tabname;
       x_count number;
       
       begin
         for xx_tabname in c_tabname loop
           open c_tab_count(xx_tabname.table_name);
              fetch c_tab_count into x_count;
           close c_tab_count;
        insert into sl_table_count(tab_name,tab_anzahl)
                             values(xx_tabname.table_name,
                                    x_xount);
          end loop;
      end;
      */
 ------------------------------------------
 -- Nettowert Rechnung Kopf
 function ermittle_rk_netto(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number) return number is
   
   cursor c_netto is
      select nvl(r.info1_num*nvl(tageskurs,1),r.rebetnetto*nvl(r.tageskurs,1)) netto
      from rechnung r 
      where r.renr = i_renr
      and r.rechnung_art = i_rechnung_art;
      
     x_netto number; 
      
   begin
      open c_netto; 
          fetch  c_netto into x_netto;
      close c_netto;
      
      if substr(i_rechnung_art,1,1) = 'G' then
        x_netto := x_netto*-1;
      end if;
      
      return(x_netto);
  end;   
  
   -- Steuerbetrag Rechnung Kopf
 function ermittle_rk_steuer(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number) return number is
   
   cursor c_steuer is
      select nvl(r.info2_num*nvl(r.tageskurs,1),nvl(r.steuer_betrag,0)*nvl(r.tageskurs,1)) netto
      from rechnung r 
      where r.renr = i_renr
      and r.rechnung_art = i_rechnung_art;
      
     x_steuer number; 
      
   begin
      open c_steuer; 
          fetch  c_steuer into x_steuer;
      close c_steuer;
      
      if substr(i_rechnung_art,1,1) = 'G' then
        x_steuer:= x_steuer*-1;
      end if;
      
      return(x_steuer);
  end; 
  -------------------------------
   -- Nettowert Rechnung-Pos
 function ermittle_rp_netto(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number,i_posnr in number) return number is
   
   cursor c_netto is
      select nvl(r.info2_num,r.pos_wert_netto*nvl(r.tageskurs,1)) netto
      from rechpos r 
      where r.renr = i_renr
      and r.rechnung_art = i_rechnung_art
      and r.posnr = i_posnr;
      
     x_netto number; 
      
   begin
      open c_netto; 
          fetch  c_netto into x_netto;
      close c_netto;
      
      if substr(i_rechnung_art,1,1) = 'G' then
        x_netto := x_netto*-1;
      end if;
      
      return(x_netto);
  end;   
-----------------------------------------------------------  
   -- Steuerbetrag Rechnung-Pos
 function ermittle_rp_steuer(i_rechnung_art in varchar2,i_rechnung_jahr in number, i_renr in number,i_posnr in number) return number is
   
   cursor c_steuer is
      select nvl(r.info3_num*nvl(r.tageskurs,1),nvl(r.steuer_betrag,0)*nvl(r.tageskurs,1)) netto
      from rechpos r 
      where r.rechnung_art = i_rechnung_art
      and r.rechnung_jahr = i_rechnung_jahr
      and r.renr = i_renr
      and r.posnr = i_posnr;
      
     x_steuer number; 
      
   begin
      open c_steuer; 
          fetch  c_steuer into x_steuer;
      close c_steuer;
      
      if substr(i_rechnung_art,1,1) = 'G' then
        x_steuer:= x_steuer*-1;
      end if;
      
      return(x_steuer);
  end;   
  
  -- ############################################################
  -- sl_ermittle_e_mail_adresse 
  -- Kundenspezifische zusatzfunktion fur drucken.ermittle_e_mail_adresse
  -- ############################################################
  
  function sl_ermittle_e_mail_adresse(i_tabelle    in varchar2,
                                   i_schluessel in varchar2,
                                   i_protok     in varchar2) return varchar2 is

    -- SIK-Speditionsauftrag lesen
    cursor c_sped is
      select a.e_mail
        from gepa_ansprech_partner a
       where a.gepa_c1 = 'L'
       and a.kdnr = (select nvl(a.text,wert) text
                       from SIVAS.KLAS_AUSPRAEGUNG a
                      where a.kenner = 'K_AUFKOPF'
                        and a.leiste = 'Speditionsauftrag'
                        and a.lfd_nr = 664
                        and a.schluessel = i_schluessel)
       and a.partner_nr = (select nvl(a.text,wert) text
                       from SIVAS.KLAS_AUSPRAEGUNG a
                      where a.kenner = 'K_AUFKOPF'
                        and a.leiste = 'Speditionsauftrag'
                        and a.lfd_nr = 722
                        and a.schluessel = i_schluessel);
    
    --manueller Speditions-Ansprechpartner_JN
    cursor c_map_jn is
      select nvl(a.text,wert) text
                       from SIVAS.KLAS_AUSPRAEGUNG a
                      where a.kenner = 'K_AUFKOPF'
                        and a.leiste = 'Speditionsauftrag'
                        and a.lfd_nr = 807
                        and a.schluessel = i_schluessel;
    x_map_jn varchar2(100);    
    
    --manueller Speditions-Ansprechpartner-Email
    cursor c_map_mail is
      select nvl(a.text,wert) text
                       from SIVAS.KLAS_AUSPRAEGUNG a
                      where a.kenner = 'K_AUFKOPF'
                        and a.leiste = 'Speditionsauftrag'
                        and a.lfd_nr = 817
                        and a.schluessel = i_schluessel;             

    xmail varchar2(100);
    
    cursor c_calc_mail is
      select a.e_mail
        from pde_persstamm a
       where a.kurzz = (select sl_pdb_v2.ermittle_pro_vk(substr(i_schluessel,7,7)) from sys.dual);

  begin

    if i_tabelle = 'K_AUF_KOPF_SPEDAUFTR' then -- SIK-Speditionsauftrag
      open c_map_jn;
      fetch c_map_jn into x_map_jn;  
      close c_map_jn;
    
      if x_map_jn = 'Ja' then
        open c_map_mail;
          fetch c_map_mail into xmail;
        close c_map_mail;
      else
        open c_sped;
          fetch c_sped into xmail;
        close c_sped;
      end if;
    elsif i_tabelle = 'AUFTRAG_KALK_DEF' then
      open c_calc_mail;
        fetch c_calc_mail into xmail;
      close c_calc_mail;
    end if;
    
    protok('sl_ermittle_e_mail_adresse '||xmail,'N');

    return xmail;

  end;
  
    -- ############################################################
  -- ermittle_e_mail_betreff 
  -- Kundenspezifische zusatzfunktion fur drucken.ermittle_e_mail_betreff
  -- ############################################################
  
  function ermittle_e_mail_betreff(i_tabelle    in varchar2,
                                  i_schluessel in varchar2,
                                  i_sprache in varchar2,
                                  i_protok     in varchar2) return varchar2 is

    cursor c_projekt_anr is
      select a.anr from wrkord a
       where substr(a.schluessel,1,13) = substr(i_schluessel,1,13); 

    x_betr varchar2(1000);
    
  begin

    if i_tabelle = 'AUFTRAG_KALK_DEF' then 
      open c_projekt_anr;
      fetch c_projekt_anr into x_betr;  
      close c_projekt_anr;

      x_betr := 'Nachkalkulation Projekt ' || x_betr;

    end if;
    
    protok('ermittle_e_mail_betreff '||x_betr,'N');

    return x_betr;

  end;
  
    ---------------------------------------
  --- ermittelt aus prodauftr den obersten Knoten (gleiche A_ART, au?er bei TN)
  -----------------------------------------
   function ermittle_obersten_knoten(i_prodauftr in varchar2) return varchar is
   
   cursor c_prod is
          select 
          decode(substr(a.prodauftr,1,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,1),substr(a.prodauftr,1,instr(a.prodauftr,'*')-1)) prod
          from wrkord a
          where a.prodauftr = i_prodauftr;
          
    cursor c_tn is  
       select  decode(substr(a.prodauftr,1,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,1),substr(a.prodauftr,1,instr(a.prodauftr,'*')-1)) prod
          from allocs a
          where a.linkab = i_prodauftr;      
      
     x_prod varchar2(25); 
      
   begin

      if substr(i_prodauftr,1,2) = 'TN' then
         open c_tn; 
            fetch  c_tn into x_prod;
         close c_tn;
      else   
         open c_prod; 
            fetch  c_prod into x_prod;
         close c_prod;
      end if;   
        
      return(x_prod);
  end;  
  
  
  -------
/*    cursor c_tn is   
       select  decode(substr(a.prodauftr,7,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,7),substr(a.prodauftr,7,instr(a.anr,'*')-1)) anr
          from allocs a
          where a.linkab = i_prodauftr;    
      
     x_anr varchar2(20); 
      
   begin
      if substr(i_prodauftr,1,2) = 'TN' then
         open c_tn; 
            fetch  c_tn into x_anr;
         close c_tn;
      else
            open c_anr; 
                fetch  c_anr into x_anr;
            close c_anr;
      end if;*/
  -------

      ---------------------------------------
  --- ermittelt aus prodauftr die obersten Bezeichnung (gleiche A_ART)
  -----------------------------------------
   function ermittle_oberste_bez(i_prodauftr in varchar2) return varchar is
   
   cursor c_prod is
          select 
          decode(substr(a.prodauftr,1,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,1),substr(a.prodauftr,1,instr(a.prodauftr,'*')-1)) prod
          from wrkord a
          where a.prodauftr = i_prodauftr;
      
     x_prod varchar2(25); 
     x_bez varchar2(50);
     
     cursor c_bez(ic_prodauftr in varchar2) is
     select w.bez 
     from wrkord w
     where w.prodauftr = ic_prodauftr;
      
   begin
      open c_prod; 
          fetch  c_prod into x_prod;
      close c_prod;
      
      open c_bez(x_prod);
        fetch c_bez into x_bez;
      close c_bez;  
        
      return(x_bez);
  end;  
        ---------------------------------------
  --- ermittelt aus prodauftr die oberste Teilenr (gleiche A_ART)
  -----------------------------------------
   function ermittle_oberste_teilenr(i_prodauftr in varchar2) return varchar is
   
   cursor c_prod is
          select 
          decode(substr(a.prodauftr,1,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,1),substr(a.prodauftr,1,instr(a.prodauftr,'*')-1)) prod
          from wrkord a
          where a.prodauftr = i_prodauftr;
      
     x_prod varchar2(25); 
     x_teilenr varchar2(50);
     
     cursor c_teilenr(ic_prodauftr in varchar2) is
     select w.teilenr
     from wrkord w
     where w.prodauftr = ic_prodauftr;
      
   begin
      open c_prod; 
          fetch  c_prod into x_prod;
      close c_prod;
      
      open c_teilenr(x_prod);
        fetch c_teilenr into x_teilenr;
      close c_teilenr;  
        
      return(x_teilenr);
  end;  
  ---------------------------------------
  --- ermittelt aus prodauftr die Kopf-Anr
  -----------------------------------------
 
    function ermittle_kopf_anr(i_prodauftr in varchar2) return varchar is
   
   cursor c_anr is
          select 
          decode(substr(a.prodauftr,7,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,7),substr(a.prodauftr,7,instr(a.anr,'*')-1)) anr
          from wrkord a
          where a.prodauftr = i_prodauftr;
          
   cursor c_tn is   
       select  decode(substr(a.prodauftr,7,instr(a.prodauftr,'*')-1),null,substr(a.prodauftr,7),substr(a.prodauftr,7,instr(a.anr,'*')-1)) anr
          from allocs a
          where a.linkab = i_prodauftr;    
      
     x_anr varchar2(20); 
      
   begin
      if substr(i_prodauftr,1,2) = 'TN' then
         open c_tn; 
            fetch  c_tn into x_anr;
         close c_tn;
      else
            open c_anr; 
                fetch  c_anr into x_anr;
            close c_anr;
      end if;
        
      return(x_anr);
  end;   
  
  -----------------------
  --ermittelt aus prodauftr die Maschinen-Nr und die Bezeichnung
  -------------------------------
  function ermittle_kopf_anr_und_bez(i_prodauftr in varchar2) return varchar is
  
  x_text varchar2(100);  
  x_prodauftr varchar2(30);
  
  begin
    
     x_prodauftr := ermittle_obersten_knoten(i_prodauftr);
          x_text := sl_allgemein.ermittle_kopf_anr(x_prodauftr)||' - '||sl_allgemein.ermittle_oberste_bez(x_prodauftr);
    return(x_text);
  end;  
      
    ---------------------------------------
  --- ermittelt aus prodauftr den Status der anr (wenn Datensatz in PDB vorhanden dann pro_status ansonsten kfm, Status
  -----------------------------------------
   function ermittle_kopf_status(i_prodauftr in varchar2) return varchar is
   
   cursor c_pdb_status(ic_anr in varchar2) is
          select s.pro_status||' / PDB'
          from sl_projekt s
          where s.pro_prnr = ic_anr
          and s.pro_status is not null;
          
   cursor c_kfm_status(ic_anr in varchar2) is
          select a.a_status||' / kfm'
          from auf_kopf a
          where a.anr = to_number(ic_anr)
          and a.a_art not in 'PT';   
          
   cursor c_tech_status(ic_anr in varchar2)is
          select w.kzstatus||' / tech'
          from wrkord w
          where w.anr = ic_anr
          and w.a_art not in ('SI','SL');
                     
      
     x_status varchar2(20); 
     x_anr varchar2(20);
      
   begin
     
      x_anr := ermittle_kopf_anr(i_prodauftr);
       
      open c_pdb_status(x_anr); 
          fetch  c_pdb_status into x_status;
          if c_pdb_status%notfound then
            if is_number_num(x_anr) = 1 then
            open c_kfm_status(x_anr);
                fetch c_kfm_status into x_status;
                if c_kfm_status%notfound then
                  open c_tech_status(x_anr);
                     fetch c_tech_status into x_status;
                  close c_tech_status;   
                end if;  
            close c_kfm_status;
            end if;
          end if;       
      close c_pdb_status;
        
      return(x_status);    
  end; 
  
     ---------------------------------------
  --- ermittelt aus prodauftr die Termine , abhangig vom Eingangsparameter, kfm. immer Liefertermin
  -----------------------------------------
   function ermittle_kopf_termin(i_prodauftr in varchar2,i_terminkz in varchar2) return varchar is
   
   cursor c_pdb_termin(ic_anr in varchar2) is 
          select s.pro_kwk_j||'/'||lpad(s.pro_kwk_w,2,'0') kwk,  -- terminkz = 'K'
                 s.pro_kwsl_j||'/'||lpad(s.pro_kwsl_w,2,'0') kwsl,  -- terminkz = 'SL'
                 s.pro_kwkons_j||'/'||lpad(s.pro_kwkons_w,2,'0') kwkons,  -- terminkz = 'KONS'
                 s.pro_kwfe_j||'/'||lpad(s.pro_kwfe_w,2,'0') kwfe, -- terminkz = 'FE'
                 to_char(s.t_mont, 'YYYY/IW') mont_b, -- terminkz = 'ME'
                 to_char(s.t_etech, 'YYYY/IW') etech_b,  -- terminkz = 'ETECH_B'
                 to_char(s.t_kon,'YYYY/IW') kons_b,  -- terminkz = 'KONS_B' Konstruktion besprochen                
                 to_char(s.t_qs,'YYYY/IW') qs_b  -- terminkz = 'QS_B' QS besprochen
          from sl_projekt s
          where s.pro_prnr = ic_anr;
          xx_termin c_pdb_termin%rowtype;
          
   cursor c_kfm_termin(ic_anr in varchar2) is
          select to_char(a.liefertermin) lt
          from auf_kopf a
          where a.anr = to_number(ic_anr);       
 
     x_lt_kfm varchar2(20);
     x_lt varchar2(20);
     x_anr varchar2(20);
      
   begin
     
      x_anr := ermittle_kopf_anr(i_prodauftr);
       
      open c_pdb_termin(x_anr); 
          fetch c_pdb_termin into xx_termin;--x_kwk,x_kwsl,x_kwkons,x_kwfe,x_kwme,x_kwetech,x_kwkons_b,x_kwqs_b;
          if i_terminkz = 'K' then 
            x_lt := xx_termin.kwk;
          elsif i_terminkz = 'SL' then 
            x_lt := xx_termin.kwsl; 
          elsif i_terminkz = 'KONS' then 
            x_lt := xx_termin.kwkons; 
          elsif i_terminkz = 'FE' then 
            x_lt := xx_termin.kwfe; 
          elsif i_terminkz = 'ME' then 
            x_lt := xx_termin.mont_b;
          elsif i_terminkz = 'ETECH_B' then 
            x_lt := xx_termin.etech_b;
          elsif i_terminkz = 'KONS_B' then
            x_lt := xx_termin.kons_b;
          elsif i_terminkz = 'QS_B' then
            x_lt := xx_termin.qs_b;     
          end if;     
             
          if c_pdb_termin%notfound then
            if is_number_num(x_anr) = 1 then
              open c_kfm_termin(x_anr);
                  fetch c_kfm_termin into x_lt;
              close c_kfm_termin;
            else 
              x_lt := 'fehlt';
            end if;  
          end if;       
      close c_pdb_termin;
        
      return(x_lt);    
  end;  
      ---------------------------------------
  --- ermittelt aus prodauftr die Termine (Tagesgenau), abhangig vom Eingangsparameter, kfm. immer Liefertermin
  -----------------------------------------
     function ermittle_kopf_termin_tag(i_prodauftr in varchar2,i_terminkz in varchar2) return date is
   
   cursor c_pdb_termin(ic_anr in varchar2) is 
          select trunc(s.t_mont) me_b, -- terminkz = 'ME_B'
                 trunc(s.t_etech) etech_b,  -- terminkz = 'ETECH_B'
                 trunc(s.t_kon) kons_b,  -- terminkz = 'KONS_B' Konstruktion besprochen                
                 trunc(s.t_qs) qs_b, -- terminkz = 'QS_B' QS besprochen
                 trunc(s.t_fat) fat_b,-- terminkz 'FAT_B' FAT moglich ab
                 trunc(s.datum1) fat--terminkz 'FAT' 
          from sl_projekt s
          where s.pro_prnr = ic_anr;
          xx_termin c_pdb_termin%rowtype;
          
   cursor c_kfm_termin(ic_anr in varchar2) is
          select trunc(a.liefertermin) lt
          from auf_kopf a
          where a.anr = to_number(ic_anr);       
      

     x_lt date;
     x_anr varchar2(20);
      
   begin
     
      x_anr := ermittle_kopf_anr(i_prodauftr);
       
      open c_pdb_termin(x_anr); 
          fetch c_pdb_termin into xx_termin;
          if i_terminkz = 'ME_B' then 
            x_lt := xx_termin.me_b;
          elsif i_terminkz = 'ETECH_B' then 
            x_lt := xx_termin.etech_b;
          elsif i_terminkz = 'KONS_B' then
            x_lt := xx_termin.kons_b;
          elsif i_terminkz = 'QS_B' then
            x_lt := xx_termin.qs_b;
          elsif i_terminkz = 'FAT_B' then
            x_lt := xx_termin.fat_b;  
          elsif i_terminkz = 'FAT' then
            x_lt := xx_termin.fat;       
          end if;     
             
          if c_pdb_termin%notfound then
            if is_number_num(x_anr) = 1 then
              open c_kfm_termin(x_anr);
                  fetch c_kfm_termin into x_lt;
              close c_kfm_termin;
            else 
              x_lt := 'fehlt';
            end if;  
          end if;       
      close c_pdb_termin;
        
      return(x_lt);    
  end;  
 ---------------------------------------
 --- ermittelt aus prodauftr die Kommision 
 -----------------------------------------
   function ermittle_kommision(i_prodauftr in varchar2) return varchar is
   
   cursor c_komm is
          select w.kommision from wrkord w
          where w.prodauftr = i_prodauftr;
      
      x_kom varchar2(25);
      
   begin
      open c_komm; 
          fetch  c_komm into x_kom;
      close c_komm;
        
      return(x_kom);
  end; 
   ---------------------------------------
 --- ermittelt aus prodauftr den kfm. Auftrag (MA)
 -----------------------------------------
function ermittle_kfm_aus_prodauftr(i_prodauftr in varchar2) return varchar is
   
    cursor c_komm is
          select w.kommision from wrkord w
          where w.prodauftr = i_prodauftr;
      
      x_kom varchar2(25);
      

   cursor c_anr_ma(ic_anr in number) is
          select a.mutter_a_art||a.mutter_jahr||a.mutter_anr
          from auf_kopf a
          where a.anr = ic_anr
          and a.a_art = 'PT';
      

   
   cursor c_anr_au(ic_anr in number) is
          select a.a_art||a.jahr||a.anr
          from auf_kopf a
          where a.anr = ic_anr
          and a.a_art = 'AU';
      x_kfm varchar2(20);    
  
      
     x_anr varchar2(20); 
      
   begin
       open c_komm; 
          fetch  c_komm into x_kom;
      close c_komm;
      
      if substr(i_prodauftr,1,2) = 'AU' then
         open c_anr_au(x_kom) ;
            fetch  c_anr_au into x_kfm;
         close c_anr_au ;
      else
            open c_anr_ma(x_kom) ;
            fetch  c_anr_ma into x_kfm;
         close c_anr_ma ;
         
      end if;
      
      return(x_kfm);
  end;  
  
  ---------------
  --Projekt-Nr und Kunde ermitteln
  ----------------------------
  function ermittle_projekt_und_kunde(i_prodauftr in varchar2) return varchar is
  
   x_text varchar2(100); 
   
  cursor c_wrk is
    select w.anr||' - '||g.name1 
      from wrkord w,gepa g
     where w.a_art = 'PT' 
       and w.anr = ermittle_kommision(i_prodauftr)
       and g.gepa_c1 = 'K'
       and g.kdnr = w.kdnr; 
  
  
  
  begin
   open c_wrk;
      fetch c_wrk into x_text;
   close c_wrk;
       
    return(x_text);
  end;  
   
  ---------------------------------------
  --- ermittelt aus prodauftr und agnr die Kostenstellenbezeichnung (Kostenstelle bei intern AG, Name1 bei Auswartsarbeitsgang
  -----------------------------------------
   function ermittle_kst_bez(i_prodauftr in varchar2,i_agnr number) return varchar is
   
   cursor c_kst is
          select o.kostenst,o.codeausw
            from oipplan o
           where o.prodauftr = i_prodauftr
             and o.agnr = i_agnr;
      
     x_kst number;
     x_codeausw varchar2(1);
     
   cursor c_bez(ic_kostenst in number) is
     select c.bez from centre c 
     where c.kostenst = ic_kostenst;
     
   cursor c_name(ic_kostenst in number) is
     select 'Fa. '||g.name1 
     from gepa g
     where g.kdnr = ic_kostenst
     and g.gepa_c1 = 'L';
     
     x_kst_bez varchar2(100);    
      
   begin
      open c_kst; 
          fetch  c_kst into x_kst,x_codeausw;
      close c_kst;
      
      if x_codeausw = 'J' then
        open c_name(x_kst);
           fetch c_name into x_kst_bez;
        close c_name;
      else
        open c_bez(x_kst);
          fetch c_bez into x_kst_bez;
        close c_bez;
      end if;         
        
      return(x_kst_bez);
  end;    
  
    ---------------------------------------
  --- ermittelt aus prodauftr und agnr die Kostenstellenbezeichnung (Kostenstelle bei intern AG, Name1 bei Auswartsarbeitsgang
  -----------------------------------------
   function ermittle_kst_bez_kpl(i_bde_nr number) return varchar is
   
   cursor c_kst is
          select o.kostenst,o.codeausw
            from oipplan o
           where o.bde_nr = i_bde_nr;
      
     x_kst number;
     x_codeausw varchar2(1);
     
   cursor c_bez(ic_kostenst in number) is
     select c.kostenst||'-'||c.bez from centre c 
     where c.kostenst = ic_kostenst;
     
   cursor c_name(ic_kostenst in number) is
     select 'Fa. '||g.name1 
     from gepa g
     where g.kdnr = ic_kostenst
     and g.gepa_c1 = 'L';
     
     x_kst_bez varchar2(100);    
      
   begin
      open c_kst; 
          fetch  c_kst into x_kst,x_codeausw;
      close c_kst;
      
      if x_codeausw = 'J' then
        open c_name(x_kst);
           fetch c_name into x_kst_bez;
        close c_name;
      else
        open c_bez(x_kst);
          fetch c_bez into x_kst_bez;
        close c_bez;
      end if;         
        
      return(x_kst_bez);
  end;    
   ---------------------------------------
  --- ermittelt aus Maschinen-Nr / Projektnummer / Zubehornummer den obersten Knoten (prodauftr)
  -----------------------------------------
   function ermittle_kopf_prodauftr(i_anr in varchar2) return varchar is
   
   cursor c_prod is
          select 
          w.prodauftr
          from wrkord w
          where w.anr = i_anr
          and w.a_art not in ('SI','TZ','TN','SL');
      
     x_prodauftr varchar2(21); 
      
   begin
      open c_prod; 
          fetch  c_prod into x_prodauftr;
      close c_prod;
        
      return(x_prodauftr);
  end;    
     ---------------------------------------
  --- ermittelt aus Rechnung den kfm. Auftrag
  -----------------------------------------
   function ermittle_auftrag_aus_renr(i_renr in varchar2) return varchar is
   
   cursor c_anr is
          select 
          r.a_art,r.anr
          from rechnung r
          where r.renr = i_renr;
             
    cursor c_li_anr(ic_anr in varchar2) is  
          select a.mutter_a_art,a.mutter_anr
          from auf_kopf a where a.anr = ic_anr;
          
   x_a_art varchar2(2);
   x_anr number;
   
   begin
      open c_anr; 
          fetch  c_anr into x_a_art,x_anr;
          if x_a_art = 'LI' then
            open c_li_anr(x_anr);
               fetch c_li_anr into x_a_art,x_anr;
            close c_li_anr;
          end if;  
      close c_anr;         
        
      return(x_anr);
  end;
 ---------------------------
 -- fullen von SIK-Datensatzen: Procedure pruft, ob Datensatz vorhanden ist, Wenn JA dann update, wenn NEIN dann Insert
 -----------------------------------------------------
   procedure fuelle_sik(i_lfd_nr in number, 
                        i_schluessel in varchar2, 
                        i_text in varchar2,
                        i_wert in number,
                        i_datum in date) is
                        
    cursor c_def is
      select k.kenner,k.leiste
      from klas_merkmal_def k
      where k.lfd_nr = i_lfd_nr;   
      x_kenner varchar2(10);
      x_leiste varchar2(20);                
      
    cursor c_sik_vorh is
       select 'J' 
         from klas_auspraegung a
        where a.schluessel = i_schluessel
          and a.lfd_nr = i_lfd_nr;
    x_sik_vorh varchar2(1);

      begin
        
     open c_def;
        fetch c_def into x_kenner,x_leiste;
        if c_def%found then
       
      
            if i_text is not null then
               open c_sik_vorh;
                  fetch c_sik_vorh into x_sik_vorh;
                     if c_sik_vorh%found then
                         update klas_auspraegung a
                            set a.text = i_text,
                                a.erfasser = user,
                                a.erf_datum = sysdate
                          where a.schluessel = i_schluessel
                            and a.lfd_nr = i_lfd_nr;
                      else
                          insert into klas_auspraegung(kenner,schluessel,klasse,leiste,lfd_nr,text)
                                               values (x_kenner,i_schluessel,-1,x_leiste,i_lfd_nr,i_text);
                      end if;
               close c_sik_vorh;
            elsif i_wert is not null then
                open c_sik_vorh;
                  fetch c_sik_vorh into x_sik_vorh;
                     if c_sik_vorh%found then
                         update klas_auspraegung a
                            set a.wert = i_wert,
                                a.erfasser = user,
                                a.erf_datum = sysdate
                          where a.schluessel = i_schluessel
                            and a.lfd_nr = i_lfd_nr;
                      else
                          insert into klas_auspraegung(kenner,schluessel,klasse,leiste,lfd_nr,wert)
                                               values (x_kenner,i_schluessel,-1,x_leiste,i_lfd_nr,i_wert);
                      end if;
               close c_sik_vorh;
            elsif i_datum is not null then
              open c_sik_vorh;
                  fetch c_sik_vorh into x_sik_vorh;
                     if c_sik_vorh%found then
                         update klas_auspraegung a
                            set a.datum = i_datum,
                                a.erfasser = user,
                                a.erf_datum = sysdate
                          where a.schluessel = i_schluessel
                            and a.lfd_nr = i_lfd_nr;
                      else
                          insert into klas_auspraegung(kenner,schluessel,klasse,leiste,lfd_nr,datum)
                                               values (x_kenner,i_schluessel,-1,x_leiste,i_lfd_nr,i_datum);
                      end if;
               close c_sik_vorh;
            end if; 
         end if;
      close c_def;  
            
   end;          

-------------------------------------------------------------------------
function ermittle_parts_index(i_teilenr in varchar2) return varchar2 is

     cursor c_aend_index is
      select max(aend_index)
        from parts_aend pa
       where teilenr = i_teilenr
         and freigabe_datum is not null
         and freigabe_datum = (select max(freigabe_datum)
                                 from parts_aend
                                where teilenr = pa.teilenr
                                  and nvl(kz_gesperrt,'N') = 'N'
                                group by teilenr);     
        
        x_aend_index varchar2(1);                        

begin
  
open c_aend_index;
     fetch c_aend_index into x_aend_index;
close c_aend_index;

  return(x_aend_index);
  
end; 
--------------------------------------------------------------
function ermittle_abt_kz (i_user varchar2) return varchar2 is
 
 x_abt_kz varchar2 (20);  
 
 cursor c_abt_kz (ic_user varchar2) is
        select b.abt_kz from pde_persstamm b where b.login_user =ic_user;
        
 begin 
      open c_abt_kz(i_user);
           fetch c_abt_kz into x_abt_kz;
           if c_abt_kz%notfound then
             x_abt_kz := 'Keiner Abteilung zogeordnet';
           end if;
       close c_abt_kz;
  return x_abt_kz;
  exception 
    when others then 
      return ('Fehler - Keine Abteilung vorhanden');
 end;
----------------------------------------------------------------------------


function ermittle_allocs_index(i_prodauftr in varchar2,i_posnr in number) return varchar2 is    
    
x_aend_index varchar2(30);
   
begin
   x_aend_index := index_pck.ermittle_allocs_index(iprodauftr => i_ProdAuftr,
                                                      iposnr     => i_PosNr,
                                                      iprotok    => 'N'); 
                                                      
   return(x_aend_index);
                                                      
end;                                                          
 --------------------------------------------------   
 function ermittle_wrkord_index(i_prodauftr in varchar2) return varchar2 is    
   
 cursor c_linkab is
     select a.prodauftr,a.posnr
       from allocs a 
      where a.linkab = i_prodauftr; 
 
    x_prodauftr varchar2(30);
    x_posnr number;
    
 cursor c_kopf is
    select w.teilenr
      from wrkord w
     where w.prodauftr = i_prodauftr;
     
     x_teilenr varchar2(21); 
    
    x_aend_index varchar2(100);
    
    begin
    
    open c_linkab;
      fetch c_linkab into x_prodauftr,x_posnr;
      if c_linkab%found then 
        x_aend_index := ermittle_allocs_index(x_prodauftr,x_posnr);
      else  
         x_aend_index := index_pck.ermittle_allocs_index(iprodauftr => i_ProdAuftr,
                                                      iposnr     => -1,
                                                      iprotok    => 'N'); 
      end if;     
        
    close c_linkab; 
    
    return(x_aend_index);
    end;
----------------------------------------------------------
 --------------------------------------------------   
 function ermittle_wrk_ind(i_prodauftr in varchar2) return varchar2 is    
   
 cursor c_linkab is
     select a.prodauftr,a.posnr
       from allocs a 
      where a.linkab = i_prodauftr; 
 
    x_prodauftr varchar2(30);
    x_posnr number;
    
 cursor c_kopf is
    select w.teilenr
      from wrkord w
     where w.prodauftr = i_prodauftr;
     
     x_teilenr varchar2(21); 
    
    x_aend_index varchar2(100);
    
    begin
    
    open c_linkab;
      fetch c_linkab into x_prodauftr,x_posnr;
      if c_linkab%found then 
        x_aend_index := ermittle_allocs_index(x_prodauftr,x_posnr);
      else  
         x_aend_index := index_pck.ermittle_allocs_index(iprodauftr => i_ProdAuftr,
                                                      iposnr     => -1,
                                                      iprotok    => 'N'); 
      end if;     
        
    close c_linkab; 
    
    return(x_aend_index);
    end;


--###############################################################

function ermittle_ende_pep_termin(i_anr varchar2, i_terminart varchar2) return date is
  
cursor c_gp (i_anr varchar2) is
select w.gpnr from wrkord w
where w.a_art in ('TE', 'MZ', 'AS')
and w.anr = i_anr;

x_gpnr number;

cursor c_gppos is
select p.schluessel, p.endedatum from gppos p
where p.terminart = i_terminart
and p.gpnr = x_gpnr;

cursor c_pep(i_pep_schluessel varchar2) is
select p.bisdatum from pep_termin p
where p.schluessel = i_pep_schluessel;

x_datum_pep date;
x_enddatum date;

begin
  
open c_gp(i_anr);
     fetch c_gp into x_gpnr;
     if c_gp%notfound then
       return '';
     end if;
close c_gp;

for xx_gppos in c_gppos loop
    for xx_pep in c_pep(xx_gppos.schluessel) loop
        if c_pep%found then
           if x_datum_pep is null or x_datum_pep < xx_pep.bisdatum then
              x_datum_pep := xx_pep.bisdatum;
           end if;
           x_enddatum := x_datum_pep;
        end if;
    end loop;
    -- Hier konnte man Ende der GPPOS anzeigen falls das relevant wird
    /*if x_enddatum is null or x_enddatum <= xx_gppos.endedatum then
      x_enddatum := xx_gppos.endedatum;
    end if;*/
end loop;

  if x_enddatum is null then
    return null;
  else
    return x_enddatum;
  end if;
end;

--###############################################################
 -----------------------------------------------
 ----  Notizen ermitteln und absteigend mit ERfasser/Anderer und Datum auflisten  
 ----------------------------------------------------
function ermittle_notiz(i_schluessel varchar2, i_tabelle varchar2) return varchar is
   cursor c_notiz is
     select n.notiz,nvl(n.aenderer,n.erfasser) benutzer,nvl(n.aend_datum,n.erf_datum) datum
     from sivas_notizen n
     where n.tabelle = i_tabelle
     and n.schluessel = i_schluessel
     order by n.lfd_nr desc;  

     
  x_text varchar2(4000);    
     
  begin
    
  x_text := Null;
  
   for xx_notiz in c_notiz loop
     
       if x_text is null then 
          x_text := xx_notiz.notiz||chr(13)||
                    xx_notiz.datum||' '||xx_notiz.benutzer;
       else
          x_text := x_text ||chr(13)||chr(13)||
                    xx_notiz.notiz||chr(13)||xx_notiz.datum||' '||xx_notiz.benutzer;
       end if;
       
       
    end loop;   
    
    return(x_text);
  
  end;  
  ---------------------------------------------------

function ermittle_saegeteil(i_teilenr in varchar2) return varchar2 is

cursor c_saege is
  select 'J' from parts p
  where p.teilenr = i_teilenr
  and p.kanban_art is null
   and  ((p.mengenc = 'M' and substr(p.grnr,1,1) = 5 and p.abw_anzahl_jn = 'J')
          or
         (p.mengenc = 'Stck' and p.kzlagerm = 'J' and p.zuschnitt = 1)
        )
   and p.teilenr not in (select k.schluessel from klas_auspraegung k where k.lfd_nr = 968 and k.text = 'Ja' and k.schluessel = p.teilenr);
  
x_saege varchar2(1);


begin 
  open c_saege;
    fetch c_saege into x_saege;
      if c_saege%found then
         return('J');
      else
         return('-');
      end if;     
  close c_saege;

            
 EXCEPTION
   when NO_DATA_FOUND then
  return null; 
  
 end;
 ---------------------------------------------------------- 
function ermittle_blechteil(i_teilenr in varchar2) return varchar2 is

cursor c_blech is
  select 'J' from parts p
  where p.teilenr = i_teilenr
  and p.kanban_art is null
  and p.abw_anzahl_jn = 'J'
  and p.mengenc = 'm2'
  and p.teilenr not in (select k.schluessel from klas_auspraegung k where k.lfd_nr = 968 and k.text = 'Ja' and k.schluessel = p.teilenr);
  
x_blech varchar2(1);


begin 
  open c_blech;
    fetch c_blech into x_blech;
      if c_blech%found then
         return('J');
      else
         return('-');
      end if;     
  close c_blech;

            
 EXCEPTION
   when NO_DATA_FOUND then
  return null;   

end;

---------------------------------------- 
------------------------------------------------------------------
------ Tif ermitteln aus ALLOCS
-----------------------------------------------------------------

function ermittle_tif_allocs(i_prodauftr varchar2, i_posnr in number) return varchar2 is

  cursor c_allocs is
     select a_art, jahr, anr, posnr, teilenr, a.zeichnung_anzeige 
       from allocs a 
      where a.prodauftr = i_prodauftr
        and a.posnr = i_posnr;

xx_allocs c_allocs%rowtype;

cursor c_allocs_aend(ic_teilenr varchar2, ic_a_art varchar2, ic_jahr number, ic_anr varchar2, ic_posnr number) is
select max (aend_index) from allocs_aend
                  where a_art = ic_A_ART
                  and jahr  = ic_JAHR
                  and anr   = ic_ANR
                  and posnr = ic_POSNR;
                  
x_allocs_aend varchar2(1);                  

cursor c_parts(ic_teilenr varchar2) is
select zeichnung_anzeige from parts where teilenr = ic_teilenr;

x_zg_anzeige varchar2(4000);
x_retval varchar2(4000);

begin  

x_zg_anzeige := null;

open c_allocs;
     fetch c_allocs into xx_allocs;
close c_allocs;

if xx_allocs.zeichnung_anzeige is null then
   open c_allocs_aend(xx_allocs.teilenr, xx_allocs.a_art, xx_allocs.jahr, xx_allocs.anr, xx_allocs.posnr);
        fetch c_allocs_aend into x_allocs_aend;
        if c_allocs_aend%found then
          x_zg_anzeige := sl_allgemein.ermittle_tif_parts_aend(xx_allocs.teilenr,x_allocs_aend);
        else
          x_zg_anzeige := sl_allgemein.ermittle_tif_parts(xx_allocs.teilenr);
        end if;   
   close c_allocs_aend;
else
   x_zg_anzeige := xx_allocs.zeichnung_anzeige;
end if;

return(x_zg_anzeige);

  exception
    when others then

      return(Null);

end;
------------------------------------------------------------------
-----  Tif ermitteln aus WRKORD
-----------------------------------------------------------------

function ermittle_tif_wrkord(i_prodauftr varchar2) return varchar2 is

cursor c_linkab is
select prodauftr,posnr  from allocs a where a.linkab = i_prodauftr;

x_prodauftr varchar2(21);
x_posnr number;


cursor c_wrkord is
  select w.teilenr,
         w.aend_index 
    from wrkord w 
   where w.prodauftr = i_prodauftr;
   
   x_teilenr  varchar2(21);
   x_aend_index     varchar2(1);

x_zg_anzeige varchar2(4000);
x_retval varchar2(4000);

begin  

x_zg_anzeige := null;

   open c_linkab;
     fetch c_linkab into x_prodauftr,x_posnr;
       if c_linkab%found then
         x_zg_anzeige := sl_allgemein.ermittle_tif_allocs(x_prodauftr,x_posnr);
       else 
          open c_wrkord;
            fetch c_wrkord into x_teilenr,x_aend_index;
            if nvl(x_aend_index,'leer') = 'leer' then
               x_zg_anzeige := sl_allgemein.ermittle_tif_parts(x_teilenr); 
            else
               x_zg_anzeige := sl_allgemein.ermittle_tif_parts_aend(x_teilenr,x_aend_index);
            end if;      
          close c_wrkord;   
       end if;    
   close c_linkab;


return(x_zg_anzeige);

  exception
    when others then

      return(Null);

end;
------------------------------------------------------------------
---- Tif ermitteln von PARTS
-----------------------------------------------------------------

function ermittle_tif_parts(i_teilenr varchar2) return varchar2 is


cursor c_parts is
   select zeichnung_anzeige 
     from parts 
    where teilenr = i_teilenr;

x_zg_anzeige varchar2(4000);
x_retval varchar2(4000);

begin  

   open c_parts;
     fetch c_parts into x_zg_anzeige;  
   close c_parts;


return x_zg_anzeige;

  exception
    when others then

      return(Null);

end;

 ------------------------------------------------------------------
----  Tif ermitteln von PARTS_AEND
-----------------------------------------------------------------

function ermittle_tif_parts_aend(i_teilenr varchar2,i_aend_index in varchar2) return varchar2 is


cursor c_parts_aend is
   select a.zeichnung_anzeige
     from parts_aend a 
    where a.teilenr = i_teilenr
    and a.aend_index = i_aend_index ;

x_zg_anzeige varchar2(4000);
x_retval varchar2(4000);

begin  

   open c_parts_aend;
     fetch c_parts_aend into x_zg_anzeige;  
   close c_parts_aend;


return x_zg_anzeige;

  exception
    when others then

      return(Null);

end;

------------------------------------------------------------------
------ Anderungsindex aus ALLOCS ermitteln
-----------------------------------------------------------------

function ermittle_aend_index(i_prodauftr varchar2, i_posnr in number) return varchar2 is

  cursor c_allocs is
     select a_art, jahr, anr, posnr
       from allocs a 
      where a.prodauftr = i_prodauftr
        and a.posnr = i_posnr;

xx_allocs c_allocs%rowtype;

cursor c_allocs_aend(ic_a_art varchar2, ic_jahr number, ic_anr varchar2, ic_posnr number) is
select max (aend_index) from allocs_aend
                  where a_art = ic_A_ART
                  and jahr  = ic_JAHR
                  and anr   = ic_ANR
                  and posnr = ic_POSNR;
                  
x_allocs_aend varchar2(1);                  


x_retval varchar2(4000);

begin  


open c_allocs;
     fetch c_allocs into xx_allocs;
close c_allocs;

open c_allocs_aend(xx_allocs.a_art, xx_allocs.jahr, xx_allocs.anr, xx_allocs.posnr);
        fetch c_allocs_aend into x_allocs_aend;  
   close c_allocs_aend;


return(x_allocs_aend);

  exception
    when others then

      return(Null);

end;
------------------------------------
function ermittle_sammel_ag(i_bde_nr in number) return varchar2 is
  
/*cursor c_sammel_ag_jn is
        select 'J' from ag_zus_gefasst z
          where z.bde_nr_ueber =  ic_bde_nr;
         x_sammel_ag_jn varchar2(1); */
          
      cursor c_sammel_ag is 
         select z.bde_nr_unter,
                (select substr(o.prodauftr,7,12)||'    '||o.verricht_text 
                        from oipplan o
                        where o.bde_nr = z.bde_nr_unter) Auftrag  
           from ag_zus_gefasst z
           where z.bde_nr_ueber = i_bde_nr;
           x_bde_unter number;
           x_auftrag varchar2(50);
           x_sammel_ag varchar2(2000);
           x_anzahl number;
           
 begin
    x_anzahl := 0;
            for xx_sammel_ag in c_sammel_ag loop
              if x_anzahl = 0 then
                  x_sammel_ag := xx_sammel_ag.auftrag;
              elsif x_anzahl  < 6 then
                  x_sammel_ag := x_sammel_ag||chr(13)||
                  xx_sammel_ag.auftrag;    
              elsif x_anzahl = 6 then
                  x_sammel_ag := x_sammel_ag||chr(13)|| 
                  '......';  
              end if;    
              x_anzahl := x_anzahl+1;
            end loop; 
         
    return x_sammel_ag;
    
    exception
    when others then

      return(Null);
  
 end;
 -----------------------------
 function ermittle_sik_bez(i_lfd_nr in number) return varchar2 is
                            
     cursor c_bez is                       
        Select x.bez
          from klas_merkmal_def  x
         where x.lfd_nr = i_lfd_nr;
           
       x_bez varchar2(2000);    
  
 begin
     open c_bez;
        fetch c_bez into x_bez;
     close c_bez;
     
     return x_bez;
     
     exception
    when others then

      return(Null);   

 end;
 -----------------------------
 function ermittle_sik_wert(i_lfd_nr in number,
                            i_schluessel in varchar2) return varchar2 is
                            
     cursor c_format is
        Select x.format
          from klas_merkmal_def  x
         where x.lfd_nr = i_lfd_nr;
    x_format varchar2(1);     
                                  
                            
     cursor c_text is                       
        Select x.text
          from klas_auspraegung  x
         where x.lfd_nr = i_lfd_nr
           and x.schluessel = i_schluessel;
           
    x_wert varchar2(2000); 
       
     cursor c_zahl is                       
        Select x.wert
          from klas_auspraegung  x
         where x.lfd_nr = i_lfd_nr
           and x.schluessel = i_schluessel;
           
     cursor c_datum is
      Select x.datum
          from klas_auspraegung  x
         where x.lfd_nr = i_lfd_nr
           and x.schluessel = i_schluessel;
  
 begin
    open c_format;
       fetch c_format into x_format;
    close c_format;
    
    if x_format = 'A' then
           open c_text;
              fetch c_text into x_wert;
              if c_text%notfound or x_wert is null then
                 x_wert := '-';
              end if;  
           close c_text;
           
     elsif  x_format = 'N' then 
           open c_zahl;
               fetch c_zahl into x_wert;
               if c_zahl%notfound or x_wert is null then
                  x_wert := '-';
               end if; 
           close c_zahl;    
     
     elsif  x_format = 'D' then
           open c_datum;
              fetch c_datum into x_wert;
              if c_datum%notfound or x_wert is null then
                 x_wert := '-';
              end if;  
           close c_datum; 
     else 
       x_wert := 'Format fehlt';            
     
     end if;
     
     
     return x_wert;
     
     exception
    when others then

      return(Null);   

 end;
-----------------------------
function ermittle_sik_wert_key(i_kenner in varchar2,
                               i_schluessel in varchar2,
                               i_klasse in number,
                               i_leiste in varchar2,
                               i_lfd_nr in number) return varchar2 is
                            
     cursor c_format is
        Select x.format
          from klas_merkmal_def  x
         where x.lfd_nr = i_lfd_nr
         and x.klasse = i_klasse
         and x.leiste = i_leiste
         and x.lfd_nr = i_lfd_nr;
    x_format varchar2(1);     
                                  
                            
     cursor c_text is                       
        Select x.text
          from klas_auspraegung  x
         where x.kenner = i_kenner
         and x.schluessel = i_schluessel
         and x.klasse = i_klasse
         and x.leiste = i_leiste
         and x.lfd_nr = i_lfd_nr ;
           
    x_wert varchar2(2000); 
       
     cursor c_zahl is                       
        Select x.wert
          from klas_auspraegung  x
         where x.kenner = i_kenner
         and x.schluessel = i_schluessel
         and x.klasse = i_klasse
         and x.leiste = i_leiste
         and x.lfd_nr = i_lfd_nr;
           
     cursor c_datum is
      Select x.datum
          from klas_auspraegung  x
         where x.kenner = i_kenner
         and x.schluessel = i_schluessel
         and x.klasse = i_klasse
         and x.leiste = i_leiste
         and x.lfd_nr = i_lfd_nr;
  
 begin
    open c_format;
       fetch c_format into x_format;
    close c_format;
    
    if x_format = 'A' then
           open c_text;
              fetch c_text into x_wert;
              if c_text%notfound or x_wert is null then
                 x_wert := '-';
              end if;  
           close c_text;
           
     elsif  x_format = 'N' then 
           open c_zahl;
               fetch c_zahl into x_wert;
               if c_zahl%notfound or x_wert is null  then
                  x_wert := '-';
               end if; 
           close c_zahl;    
     
     elsif  x_format = 'D' then
           open c_datum;
              fetch c_datum into x_wert;
              if c_datum%notfound or x_wert is null  then
                 x_wert := '-';
              end if;  
           close c_datum; 
     else 
       x_wert := 'Format fehlt';            
     
     end if;
     
     
     return x_wert;
     
     exception
    when others then

      return(Null);   

 end;
-----------------------------
 function ermittle_sik_text(i_lfd_nr in number,
                            i_schluessel in varchar2) return varchar2 is
                            
     cursor c_text is                       
        Select x.text
          from klas_auspraegung  x
         where x.lfd_nr = i_lfd_nr
           and x.schluessel = i_schluessel;
           
       x_text varchar2(2000);    
  
 begin
     open c_text;
        fetch c_text into x_text;
        if c_text%notfound or x_text is null then
          x_text := '-';
        end if;  
     close c_text;
     
     return x_text;
     
     exception
    when others then

      return(Null);   

 end;
 -----------------------------
 function ermittle_sik_zahl(i_lfd_nr in number,
                            i_schluessel in varchar2) return number is
                            
     cursor c_wert is                       
        Select x.wert
          from klas_auspraegung  x
         where x.lfd_nr = i_lfd_nr
           and x.schluessel = i_schluessel;
           
       x_wert number;    
  
 begin
     open c_wert;
        fetch c_wert into x_wert;
        if c_wert%notfound or x_wert is null then
          x_wert := Null;
        end if; 
     close c_wert;
     
     return x_wert;
     
     exception
    when others then

      return(Null);   

 end; 
 -----------------------------
 function ermittle_sik_datum(i_lfd_nr in number,
                            i_schluessel in varchar2) return date is
                            
     cursor c_datum is                       
        Select x.datum
          from klas_auspraegung  x
         where x.lfd_nr = i_lfd_nr
           and x.schluessel = i_schluessel;
           
       x_datum date;    
  
 begin
     open c_datum;
        fetch c_datum into x_datum;
        if c_datum%notfound or x_datum is null then
          x_datum := Null;
        end if; 
     close c_datum;
     
     return x_datum;
     
     exception
    when others then

      return(Null);   

 end;  
 -----------------------------
 --- ermittelt SIK-Schlüssel aus Schlüssel der Tabellen (z.T. sind sik-Schlüssel und tabellen-Schlüssel nicht gleich)
 ------------------
 function ermittle_sik_schluessel(i_kenner in varchar2,
                                  i_tabelle in varchar2,
                                  i_schluessel in varchar2 ) return varchar2 is
                                  
 x_result boolean;
 o_feld_1 varchar2(200);
 o_feld_2 varchar2(200);
 o_feld_3 varchar2(200);
 o_feld_4 varchar2(200);
 o_feld_5 varchar2(200);
 o_feld_6 varchar2(200);
 o_feld_7 varchar2(200);
 o_feld_8 varchar2(200);
 o_feld_9 varchar2(200);
 
 x_sik_schluessel varchar2(2000);
 
 begin
   
   x_result := schluessel.umdrehen(i_schluessel => i_schluessel,
                                i_tabelle => i_tabelle,
                                o_feld_1 => o_feld_1,
                                o_feld_2 => o_feld_2,
                                o_feld_3 => o_feld_3,
                                o_feld_4 => o_feld_4,
                                o_feld_5 => o_feld_5,
                                o_feld_6 => o_feld_6,
                                o_feld_7 => o_feld_7,
                                o_feld_8 => o_feld_8,
                                o_feld_9 => o_feld_9,
                                i_protok => 'N');
                                
                                
   x_sik_schluessel :=  SIK.erstelle_leisten_schluessel(x_kenner => i_kenner,
                                             x_feld_1 => o_feld_1,
                                             x_feld_2 => o_feld_2,
                                             x_feld_3 => o_feld_3,
                                             x_feld_4 => o_feld_4,
                                             x_feld_5 => o_feld_5,
                                             x_feld_6 => o_feld_6,
                                             x_feld_7 => o_feld_7,
                                             x_feld_8 => o_feld_8,
                                             x_feld_9 => o_feld_9);        
                                             
   return x_sik_schluessel;
   
   end;               
                       
 
 -----------------------------------------------
 ---- Seriennummern Zuordnung ermitteln
 -----------------------------------------------
function ermittle_bezug(i_anr in varchar2) return varchar is
  
 cursor c_linkauf(ic_prodauftr in varchar2) is
  select a.prodauftr,a.posnr from allocs a
  where a.linkab = ic_prodauftr;
  
  x_prodauftr_ueb varchar2(21);
  x_posnr_ueb number;
  
  cursor c_serie(ic_prodauftr_ueb in varchar2,ic_posnr_ueb in number) is 
  select a.lfdnr,s.serien_nummer from allocs_serie_zuo a,parts_seriennummer s
  where a.prodauftr = ic_prodauftr_ueb
    and a.posnr = ic_posnr_ueb
    and s.lfdnr = a.lfdnr;
    
  x_prodauftr varchar2(21);
  x_result varchar2(2000);
  
begin
  x_prodauftr := sl_allgemein.ermittle_kopf_prodauftr(i_anr);
  if substr(x_prodauftr,1,2) in ('UA','AS') then 
     open c_linkauf(x_prodauftr);
          fetch c_linkauf into x_prodauftr_ueb,x_posnr_ueb; 
     close c_linkauf;  
      
     for xx_serie in  c_serie(x_prodauftr_ueb,x_posnr_ueb) loop
       if x_result is null then
          x_result := xx_serie.serien_nummer;
       else
          x_result:= x_result||' / '||xx_serie.serien_nummer;
       end if;  
     end loop;  
         x_result:= x_result||' / '||i_anr;
  else 
     x_result := i_anr;
  end if;  
  
  return(x_result);  
end;
------------------------------------
-----------------------------------------------
FUNCTION ermittle_merkmal(i_prodauftr     in varchar2,
                          i_posnr         in number,
                          i_leisten_nr    in number) RETURN varchar IS

  -- Version 4.0
  
  xText                  varchar2(2000);
  xWert                  varchar2(2000);

  cursor c_strukt is
    select prodauftr, posnr
      from allocs
    connect by prior prodauftr = linkab
     start with prodauftr = i_prodauftr and posnr = i_posnr;
     xx_strukt c_strukt%rowtype;
     
  -- Cursor fuer Positionsmerkmale
  cursor c_pos_merk(ic_prodauftr in varchar2,ic_posnr in number) is
    select to_char(wert), text
      from auf_merk
     where prodauftr = ic_prodauftr
       and posnr = ic_posnr
       and leisten_nr = i_leisten_nr;
  
  -- Cursor fuer Kopfmerkmale
  cursor c_kopf_merk(ic_prodauftr in varchar2) is
    select to_char(wert), text
      from auf_merk
     where prodauftr = ic_prodauftr
       and posnr <= 0
       and leisten_nr = i_leisten_nr;

BEGIN

  for xx_strukt in c_strukt loop
    -- zuerst ziehen Merkmale an der Position
    open c_pos_merk(xx_strukt.prodauftr,xx_strukt.posnr);
       fetch c_pos_merk into xWert, xText;
       exit when c_pos_merk%found;
   close c_pos_merk;

    open c_kopf_merk(xx_strukt.prodauftr);
       fetch c_kopf_merk into xWert, xText;  
    close c_kopf_merk;

  end loop;

    return nvl(xwert,xtext);
EXCEPTION
   when NO_DATA_FOUND then
        return(0); 
END;
----------------------
FUNCTION ermittle_merkmal_aufruf(i_prodauftr     in varchar2,
                              i_posnr         in number,
                              i_leisten_nr    in number) RETURN varchar IS


  
  xText                  varchar2(2000);
  xWert                  varchar2(2000);
  x_prodauftr            varchar2(20);
  x_posnr                number;

  cursor c_strukt is
    select prodauftr, posnr
      from allocs
    connect by prior prodauftr = linkab
     start with prodauftr = i_prodauftr and posnr = i_posnr;
     xx_strukt c_strukt%rowtype;
     
  -- Cursor fuer Positionsmerkmale
  cursor c_pos_merk(ic_prodauftr in varchar2,ic_posnr in number) is
    select a.prodauftr,a.posnr 
      from auf_merk a
     where prodauftr = ic_prodauftr
       and posnr = ic_posnr
       and leisten_nr = i_leisten_nr;
  
  -- Cursor fuer Kopfmerkmale
  cursor c_kopf_merk(ic_prodauftr in varchar2) is
    select  a.prodauftr
      from auf_merk a
     where prodauftr = ic_prodauftr
       and posnr = -1
       and leisten_nr = i_leisten_nr;

BEGIN

  for xx_strukt in c_strukt loop
    -- zuerst ziehen Merkmale an der Position
    open c_pos_merk(xx_strukt.prodauftr,xx_strukt.posnr);
       fetch c_pos_merk into x_prodauftr,x_posnr;
       xtext := 'Y:\NetEnv\SnfStarter.exe instance=servolinux program=Technik PRODAUFTR='||x_prodauftr||' POSNR='||x_posnr;
       exit when c_pos_merk%found;
   close c_pos_merk;

    open c_kopf_merk(xx_strukt.prodauftr);
       fetch c_kopf_merk into x_prodauftr;  
       xtext := 'Y:\NetEnv\SnfStarter.exe instance=servolinux program=Technik PRODAUFTR='||x_prodauftr;
    close c_kopf_merk;

  end loop;
     
    return (xtext);
EXCEPTION
   when NO_DATA_FOUND then
        return(0); 
END;
-----------------------------------------------
FUNCTION ermittle_merkmal_kfm(i_a_art         in varchar2,
                              i_jahr               in number,
                              i_anr             in number,
                              i_posnr           in number,
                              i_leisten         in number
                          ) RETURN varchar IS

 
  x_wert   number;               
  x_text   varchar2(2000); 
  x_format varchar2(1);              
  

     
  -- Cursor fuer Positionsmerkmale
  cursor c_merk_kfm is
    select a.wert, a.text,m.format
      from auf_merk_kfm a, merkmal m
     where a.a_art = i_a_art
       and a.jahr = i_jahr
       and a.anr = i_anr
       and a.posnr = i_posnr
       and a.leisten_nr = i_leisten
       and m.leisten_nr = a.leisten_nr;
  
BEGIN
  
    open c_merk_kfm;
        fetch c_merk_kfm into  x_Wert, x_Text,x_format;
        if c_merk_kfm%found then
           if x_format = 'A' then
              return (x_text);
           else
              return to_char(x_wert);
           end if;
        else
          return('?');
        end if;    

    close c_merk_kfm;
      
 
      
EXCEPTION
   when NO_DATA_FOUND then
        return('-'); 
END;
------------------------------

function ermittle_user_zu_rolle(i_rolle varchar2) return boolean is
  
cursor c_rolle is
select 'J' from sivas_user_rolle t
where t.grantee = user
and t.granted_role = i_rolle;

x_rolle varchar2(1);

begin
  open c_rolle;
       fetch c_rolle into x_rolle;
       if c_rolle%found then
         return true;
       else
         return false;
       end if;
  close c_rolle;
         
       
end;
---------------------------------
function ermittle_fibu_konto_bez(i_konto in varchar2) return varchar2 is
  cursor c_bez is
  select t.bemerkung
    from fibu_konten t 
   where t.art = 'ER'
     and t.konto = i_konto;
     
   x_bez varchar2(2000);
   
   begin 
     open c_bez;
        fetch c_bez into x_bez;
     close c_bez;  
   
   return x_bez;   
   
     exception
    when others then

      return(Null);  
   end;   
          
-------------------------------

function ermittle_reklamation(i_prodauftr varchar2) return number is
  
cursor c_rekla is
    select m.rw_lfd_nr
    from rekl_wunsch_massnahmen m, wrkord w
    where w.prodauftr = i_prodauftr
    and  m.schluessel = w.schluessel
    and m.tabelle = 'WRKORD'
    group by rw_lfd_nr;
    
x_rekla number;
    
begin
    if substr(i_prodauftr,1,2) in ('TN', 'AU') then
      open c_rekla;
           fetch c_rekla into x_rekla;
      close c_rekla;
    end if;
    
return nvl(x_rekla,null);
    
end;

-------------------------------

 procedure historie_favorit_schreiben(i_tabelle in varchar2,i_schluessel in varchar2) is
 
 cursor c_bk is
 select bk.rowid,
        bk.a_art||' '||bk.jahr||' '||bk.anr||' '||bk.name1||' '||bk.name1||' '||bk.name2||' '||bk.erf_datum ibeschreibung 
        from bes_kopf bk where bk.schluessel = i_schluessel;
   
 cursor c_ak is
 select ak.rowid,
        ak.a_art||' '||ak.jahr||' '||ak.anr||' '||ak.name1||' '||ak.name1||' '||ak.name2||' '||ak.erf_datum ibeschreibung 
        from auf_kopf ak where ak.schluessel = i_schluessel;
        
 cursor c_parts is
 select p.rowid,
        p.teilenr||' '||p.bez ibeschreibung 
        from parts p where p.schluessel = i_schluessel; 
 
 cursor c_gepa is
 select g.rowid,
        g.kdnr||' '||g.name1 ibeschreibung 
        from gepa g where g.schluessel = i_schluessel; 
        
 cursor c_wrkord is
 select w.rowid,
        w.prodauftr||' '||w.teilenr ibezeichnung
        from wrkord w where w.schluessel = i_schluessel;            
         
        
 x_rowid varchar2(2000);
 x_beschreibung varchar2(2000);       
    
     
   begin
     if i_tabelle = 'BES_KOPF' then
        open c_bk;
           fetch c_bk into x_rowid,x_beschreibung;
        close c_bk;
     elsif i_tabelle = 'AUF_KOPF' then
        open c_ak;
           fetch c_ak into x_rowid,x_beschreibung;
        close c_ak;
     elsif i_tabelle = 'PARTS' then
        open c_parts;
           fetch c_parts into x_rowid,x_beschreibung;
        close c_parts;  
     elsif i_tabelle = 'GEPA' then
        open c_gepa;
           fetch c_gepa into x_rowid,x_beschreibung;
        close c_gepa;
     elsif i_tabelle = 'WRKORD' then
        open c_wrkord;
           fetch c_wrkord into x_rowid,x_beschreibung;
        close c_wrkord;        
     end if;                         
     
          
 
    allgemein.history_favorit_schreiben(itabelle      => i_tabelle,
                                        irowid        => x_rowid,
                                        ityp          => 'F',
                                        ibeschreibung => x_beschreibung);
     
  end;   
  
  ------------------------------------------------------------------
   procedure historie_favorit_loeschen(i_tabelle in varchar2,i_schluessel in varchar2) is
 
 cursor c_bk is
 select bk.rowid
        from bes_kopf bk where bk.schluessel = i_schluessel;
   
 cursor c_ak is
 select ak.rowid
        from auf_kopf ak where ak.schluessel = i_schluessel;
        
 cursor c_parts is
 select p.rowid
        from parts p where p.schluessel = i_schluessel; 
 
 cursor c_gepa is
 select g.rowid
        from gepa g where g.schluessel = i_schluessel;  

 cursor c_wrkord is
 select w.rowid
        from wrkord w where w.schluessel = i_schluessel;                   
         
        
 x_rowid varchar2(2000);
 
    
     
   begin
     if i_tabelle = 'BES_KOPF' then
        open c_bk;
           fetch c_bk into x_rowid;
        close c_bk;
     elsif i_tabelle = 'AUF_KOPF' then
        open c_ak;
           fetch c_ak into x_rowid;
        close c_ak;
     elsif i_tabelle = 'PARTS' then
        open c_parts;
           fetch c_parts into x_rowid;
        close c_parts;  
     elsif i_tabelle = 'GEPA' then
        open c_gepa;
           fetch c_gepa into x_rowid;
        close c_gepa;
     elsif i_tabelle = 'WRKORD' then
        open c_wrkord;
           fetch c_wrkord into x_rowid;
        close c_wrkord;        
     end if;    
          
 
    allgemein.history_favorit_loeschen(itabelle      => i_tabelle,
                                        irowid        => x_rowid,
                                        ityp          => 'F');

   end;                                     
       
  -------------------------------------
  -- ermittelt Summe stueckzeit_ist aus oipplan / i_kenner:  E = Kostenstelle einzeln z.B. 103 (103,104) / G = Kostenstellengruppe z.B. 10 = 10%
  -------------------------------------------
   function ermittle_stueckzeit_ist(i_prodauftr in varchar2,i_kostenst in varchar2,i_kenner in varchar2) return number is
   
 cursor c_zeit is
    select sum(o.stkzeitist)
    from oipplan o
    where o.prodauftr like i_prodauftr||'%'
    and o.kostenst in i_kostenst;
    
    type konfig_cursor is ref cursor;
    c_cursor konfig_cursor;
   
    
    x_zeit number;
    x_cursor varchar2(2000);
    
    begin 
    
   if i_kenner = 'G' then  --  E = Kostenstelle einzeln / G = Kostenstellengruppe z.B. 10 = 10%
      x_cursor:= 'select sum(o.stkzeitist)
                  from oipplan o
                  where o.prodauftr in (select w.prodauftr from wrkord w
                                        connect by prior w.prodauftr = w.linkauf
                                        start with w.prodauftr = '''||i_prodauftr||''')'||
                   'and o.kostenst like '''||i_kostenst||'%'''; 
                  
   else
    
      x_cursor:= 'select sum(o.stkzeitist)
                  from oipplan o
                  where o.prodauftr in (select w.prodauftr from wrkord w
                                        connect by prior w.prodauftr = w.linkauf
                                        start with w.prodauftr = '''||i_prodauftr||''')'||
                  ' and o.kostenst in '||i_kostenst;  
   end if; 
    
  open c_cursor FOR x_cursor;
  
   fetch c_cursor into x_zeit;
    
  close c_cursor; 
  return(x_zeit);   
  
    end;   
    --------------------------

------------------------------------------------------------
-- Ermitteln des Feldes "Ersatz fur" im Teilestamm
-- Ersatz fur ist keine Spalte in Parts sondern wird beim Aufruf ermittelt
------------------------------------------------------------ 

function ermittle_ersatz_fuer(i_teilenr varchar2) return varchar2 is
  
cursor c_ersatz_fuer is
    select teilenr from parts 
    where verknummer = i_teilenr;

x_ersatz_fuer varchar2(255);

begin

open c_ersatz_fuer;
     fetch c_ersatz_fuer into x_ersatz_fuer;
close c_ersatz_fuer;

return nvl(x_ersatz_fuer,null);
    
end;

------------------------------------------------------------
-- ermittelt ob die Allocs-Position eine Verlagerung ist
-----------------------------------------------------------

  function ermittle_verlagerung(i_prodauftr in varchar2,i_posnr in number) return varchar is
  
   cursor c_verl is
   select 'J' from allocs_beschaf a
   where a.prodauftr = i_prodauftr 
   and a.posnr = i_posnr
   and nvl(a.verlagerungs_art,'KB') != 'KB';
   x_verl varchar2(1);
   
   begin
     open c_verl;
       fetch c_verl into x_verl;
     close c_verl;
     
   return(x_verl);   
    
     
 end; 
------------------------------------------------------------

function ermittle_warenstat_ek(i_teilenr in varchar2) return varchar2 is
   
   cursor c_parts is
   select p.waren_stat_nr,
          p.info6 from parts p
   where p.teilenr = i_teilenr;
   
   cursor c_warenstat_ref(ic_teilenr_ref in varchar2) is
    select  p.waren_stat_nr from parts p
    where p.teilenr = ic_teilenr_ref;
  
   x_warenstat varchar2(20);
   x_info6 varchar2(21);
   x_erg varchar2(20);
  begin
    
  x_warenstat := sl_allgemein.ermittle_sik_text(i_lfd_nr     => 1039,
                                                      i_schluessel => i_teilenr);
                                                      
  
  if x_warenstat = '-' then
    open c_parts;
      fetch c_parts into x_warenstat,x_info6;
      if x_warenstat is  null then
        if x_info6 is not null then
          open c_warenstat_ref(x_info6);
             fetch c_warenstat_ref into x_warenstat;
          close c_warenstat_ref;
        end if;     
      
      end if;   
    close c_parts;      
      
    
  end if;   
  
  x_erg := nvl(x_warenstat,'fehlt'); 
    
  return x_erg;
     
end;
---------------
    
function ermittle_lagerkenner(i_teilenr in varchar2) return varchar2 is     
      
  
  cursor c_parts is 

    select a.kzlagerm,a.kzkauf,a.status
    from parts a 
    where a.teilenr = i_teilenr;
    
    x_kzlagerm varchar2(1);
    x_kzkauf varchar2(1); 
    x_status number;
    
    x_freigabe varchar2(4);
    x_result varchar2(100);
    
   begin
   
   open c_parts;
       fetch c_parts into x_kzlagerm, x_kzkauf, x_status; 
   close c_parts;       
          
        if x_status = 9 then
          x_result := 'Teil gesperrt !!!' ;
        elsif substr(i_teilenr,1,2) = '50' then 
          x_result := Null ;
        elsif x_kzlagerm = 'J' then
          x_result := 'Lagerteil' ;
        elsif x_kzkauf = 'E' then  
          x_freigabe :=    sl_allgemein.ermittle_sik_text(1261,i_teilenr);
          if x_freigabe = 'Ja' then
            x_result := 'Lagerteil' ;
          else
            x_result := 'Eigenbauteil' ; 
          end if;   
        elsif x_kzkauf = 'K' then
          x_result := 'Kaufteil' ;  
        end if;   

        return x_result;
        
  end ermittle_lagerkenner;     
   ------------------  

function ermittle_ersetzt_durch_loop(i_teilenr varchar2) return varchar2 is
   
cursor c_teilenr is     
select distinct last_value(p.teilenr) 
over(ORDER BY level RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) AS "LASTVALUE"
  from parts p
connect by prior p.verknummer = p.teilenr
 start with p.teilenr = i_teilenr;
 
x_retval varchar2(255);
 
begin

open c_teilenr;
     fetch c_teilenr into x_retval;
close c_teilenr;
   
/* Gibt 1 zuruck wenn gleich und 0 wenn ungleich. Sind beide Felder NULL gelten sie als gleich*/
if allgemein.gleich(i_teilenr, x_retval) = 1 then
  x_retval := null;
end if;

return x_retval;

end;


----------------------------------------------------------------
----------------------------------------------------------------
--> Preisfindung
----------------------------------------------------------------
----------------------------------------------------------------
function ermittle_vk_preis(i_teilenr varchar2, i_jahre_zurueck varchar2) return number is

cursor c_ek_preis (ic_teilenr varchar2) is
select p.letzter_ek_preis, p.letzter_ek_preis_datum from parts p
where p.teilenr = ic_teilenr
and p.letzter_ek_preis_datum > sysdate - i_jahre_zurueck * 365;

x_ek_preis_num number;
x_ek_preis_dat date;
x_retval number;

x_grenze_1 number;
x_grenze_2 number;
x_grenze_3 number;
x_grenze_4 number;

x_faktor_1 number;
x_faktor_2 number;
x_faktor_3 number;
x_faktor_4 number;

begin

x_grenze_1 := 0;
x_grenze_2 := 50;
x_grenze_3 := 500;
x_grenze_4 := 5000;

x_faktor_1 := 4;
x_faktor_2 := 2.4;
x_faktor_3 := 1.5;
x_faktor_4 := 1.2;

open c_ek_preis(i_teilenr);
     fetch c_ek_preis into x_ek_preis_num, x_ek_preis_dat;
        if c_ek_preis%found then
        
           --> Definition mit "Zahlen"
            /*if x_ek_preis_num <= 50 then
               x_retval := x_ek_preis_num * (4 - ((x_ek_preis_num - 0) / (50-0)) * (4 - 2.4)); -- x_ek_preis_num * (4 - x_ek_preis_num * 1.6 / 50)
            elsif x_ek_preis_num > 50 and x_ek_preis_num <= 500 then
               x_retval := x_ek_preis_num * (2.4 - ((x_ek_preis_num - 50) / (500-50)) * (2.4 - 1.5));
            elsif x_ek_preis_num > 500 and x_ek_preis_num <= 5000 then
               x_retval := x_ek_preis_num * (1.5 - ((x_ek_preis_num - 500) / (5000-500))  * (1.5 - 1.2));
            elsif x_ek_preis_num > 5000 then
               x_retval := x_ek_preis_num * 1.2;  */
        
            if x_ek_preis_num <= x_grenze_2 then
               x_retval := x_ek_preis_num * (x_faktor_1 - ((x_ek_preis_num - x_grenze_1) / (x_grenze_2 - x_grenze_1)) * (x_faktor_1 - x_faktor_2)); -- x_ek_preis_num * (4 - x_ek_preis_num * 1.6 / 50)
            elsif x_ek_preis_num > x_grenze_2 and x_ek_preis_num <= x_grenze_3 then
               x_retval := x_ek_preis_num * (x_faktor_2 - ((x_ek_preis_num - x_grenze_2) / (x_grenze_3 - x_grenze_2)) * (x_faktor_2 - x_faktor_3));
            elsif x_ek_preis_num > x_grenze_3 and x_ek_preis_num <= x_grenze_4 then
               x_retval := x_ek_preis_num * (x_faktor_3 - ((x_ek_preis_num - x_grenze_3) / (x_grenze_4 - x_grenze_3)) * (x_faktor_3 - x_faktor_4));
            elsif x_ek_preis_num > x_grenze_4 then
               x_retval := x_ek_preis_num * x_faktor_4;
            end if;
        else
            x_retval := 0;
        end if;
close c_ek_preis;

return round(x_retval,2);

end;

----------------------------------------------------------------
----------------------------------------------------------------

function ermittle_grundpreis(i_teilenr varchar2, i_preis_art varchar2 default 'N') return number is
  
cursor c_preis is
   select t.preis
     from GRUNDPREIS t
    where t.ab_datum = (select max(a.ab_datum) from grundpreis a where a.teilenr = t.teilenr)
      and t.teilenr = i_teilenr
      and t.preis_art = i_preis_art;

x_retval number;

begin
  
open c_preis;
     fetch c_preis into x_retval;
       if c_preis%notfound then 
          x_retval := 0;
       end if;
close c_preis;

return x_retval;
end;


   ------------------  

procedure setze_grundpreis(i_teilenr varchar2, 
                           i_grundpreis number, 
                           i_ab_datum date default trunc(sysdate), 
                           i_preis_art varchar2 default 'N', 
                           omeldung out varchar2) is

cursor c_parts is
select p.mengenc from parts p where p.teilenr = i_teilenr and p.mengenc is not null;

x_PREISEINH varchar2(255);

begin
open c_parts;
     fetch c_parts into x_PREISEINH;
     if c_parts%found then
        -- Teil mit Preis gefunden --> Loschen eventuell bestehender Satze fur heute
        delete from grundpreis_staffel
          where teilenr = i_teilenr
            and preis_art = i_preis_art
            and trunc(ab_datum) = trunc(i_ab_datum);
        
        delete from grundpreis
          where teilenr = i_teilenr
            and preis_art = i_preis_art
            and trunc(ab_datum) = trunc(i_ab_datum);
            
         commit;
        
         INSERT INTO GRUNDPREIS(TEILENR,PREIS_ART,AB_DATUM,PREIS,MANU,RABATT_JN,PREISINH,PREISEINH,ERFASS_DAT,ERFASS_USER) 
                        VALUES (i_teilenr, i_preis_art, i_ab_datum, i_grundpreis, 1, 'J', 1, 'Stck',sysdate,user);
         omeldung := 'Grundpreis erfolgreich im Teilestamm hinterlegt';
     end if;
close c_parts;

EXCEPTION when others then
 omeldung := 'Fehler beim setzen des Grunpreis im Teilestamm';
 rollback;
end;
--------------------------------------------------------------
function ermittle_status (i_status in number) return varchar2 is
 
 x_status varchar2 (50);  
 
 cursor c_status  is
        select s.kz ||' - '||s.kztext 
          from status s 
         where s.kz = i_status;
        
 begin 
      open c_status;
           fetch c_status into x_status;
      close c_status;
  return x_status;
  exception 
    when others then 
      return ('Fehler - Keine Status vorhanden');
 end;
 
 --------------------------------------------------------------
function ermittle_absagegrund (i_status in number) return varchar2 is
 
 x_status varchar2 (50);  
 
 cursor c_status  is
        select s.id ||' - '||s.grund
          from projekt_absagegruende_stamm s 
         where s.id = i_status;
        
 begin 
      open c_status;
           fetch c_status into x_status;
      close c_status;
  return x_status;
  exception 
    when others then 
      return ('Fehler - Kein Absagegrund vorhanden');
 end;

--###############################################################
function erm_ende_pep_termin_neumontage(i_anr varchar2) return date is
  
cursor c_gp (ic_anr varchar2) is
select k.gpnr from wrkord w, gpkopf k
where 1=1
and w.gpnr = k.gpnr
and w.a_art in ('AS')
and k.stamm_gpnr = 501
and exists
(select * from allocs_serie_zuo a, parts_seriennummer s
where a.lfdnr = s.lfdnr
and s.serien_nummer = ic_anr
and a.prodauftr=w.prodauftr);

x_gpnr number;

cursor c_gppos is
select p.schluessel, p.endedatum from gppos p
where p.terminart = 'AS10'
and p.gpnr = x_gpnr;

cursor c_pep(i_pep_schluessel varchar2) is
select p.bisdatum from pep_termin p
where p.schluessel = i_pep_schluessel;

x_datum_pep date;
x_enddatum date;

begin
  
open c_gp(i_anr);
     fetch c_gp into x_gpnr;
     if c_gp%notfound then
       return '';
     end if;
close c_gp;

for xx_gppos in c_gppos loop
    for xx_pep in c_pep(xx_gppos.schluessel) loop
        if c_pep%found then
           if x_datum_pep is null or x_datum_pep < xx_pep.bisdatum then
              x_datum_pep := xx_pep.bisdatum;
           end if;
           x_enddatum := x_datum_pep;
        end if;
    end loop;
    -- Hier konnte man Ende der GPPOS anzeigen falls das relevant wird
    /*if x_enddatum is null or x_enddatum <= xx_gppos.endedatum then
      x_enddatum := xx_gppos.endedatum;
    end if;*/
end loop;

  if x_enddatum is null then
    return null;
  else
    return x_enddatum;
  end if;
end;
-----------------------
function ermittle_hauptlagerort(i_teilenr in varchar2) return varchar2 is
  
  cursor c_lager is
    select l.lagerort
    from lagerpl l
    where l.teilenr = i_teilenr
    and l.code_hauptlager = 'J';
    
    x_lagerort varchar2(200);
    
    begin
      
    open c_lager;
       fetch c_lager into x_lagerort;
    close c_lager;   
      
    return x_lagerort;
   
   end;

--###############################################################

function ermittle_kosten_prodauftr(i_prodauftr varchar2,
                                     i_pos_nr    number
                                     )
    return number is
  
    cursor c_kosten is
      Select t.ergebnis
        From AUFTRAG_KALK_DEF_POS t
       Where 1 = 1
         and t.prodauftr = i_prodauftr
         and t.pos_nr = i_pos_nr
         And t.def_nr = 10;
  
    x_kosten number;
  
  begin
    open c_kosten;
    fetch c_kosten
      into x_kosten;
    if c_kosten%notfound then
      x_kosten := 0;
    end if;
    close c_kosten;
    return x_kosten;
  end;

--###############################################################

end SL_ALLGEMEIN;
```
