# SL_A1_ANTRAG

- Typ: PACKAGE
- Extrahiert: 2026-06-16 12:53:13
- Quelle: (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.10.10.36)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=linux)))

```sql
CREATE OR REPLACE EDITIONABLE PACKAGE "SIVAS"."SL_A1_ANTRAG" is

  -- Author  : HAAS
  -- Created : 28.02.2019 13:41:13
  -- Purpose : 
  
function a1_lesen_schreiben_jn(i_user in varchar2) return varchar2;

procedure a1_ma_anzeige(i_user varchar2);

procedure a1_gepa_volltext(i_text in varchar2, i_fill_table_jn in varchar2);
procedure a1_gepa_ansprech(i_kdnr number);


procedure a1_anlegen(  i_pers_nr in number,
                             i_fam_name in varchar2,
                             i_name_vorname in varchar2,
                             i_von in varchar2,
                             i_bis in varchar2,
                             i_kdnr in number default null,
                             i_firma in varchar2,
                             i_strasse in varchar2,
                             i_plz in varchar2,
                             i_ort in varchar2,
                             i_land in varchar2,
                             i_angelegt_von in varchar2);
                             
procedure a1_bearbeiten(     i_lfdnr in number,
                             i_von in varchar2,
                             i_bis in varchar2,
                             i_strasse in varchar2,
                             i_plz in varchar2,
                             i_ort in varchar2,
                             i_user in varchar2,
                             i_newstatus in varchar2);

procedure update_delete(i_lfdnr in number, i_function in varchar2, i_user in varchar2);
procedure a1_workflow_status(i_lfdnr in number, i_oldstatus in varchar2, i_newstatus in varchar2);
end SL_A1_ANTRAG;

CREATE OR REPLACE EDITIONABLE PACKAGE BODY "SIVAS"."SL_A1_ANTRAG" is

x_return varchar2(20);

------------------------------------------------------------------------
--######################################################################
------------------------------------------------------------------------

function a1_lesen_schreiben_jn(i_user in varchar2) return varchar2 is
 
-- L_S                             = LESEN (Datensätze werden angezeigt) UND SCHREIBEN (Datensätze können angelegt u. gelöscht werden)
-- BEANTRAGT_GENEHMIGT             = BEANTRAGT und genehmigt setzen
begin
  /*x_return := 'N';
       if lower(i_user) in ('saelinger', 'junker', 'koepf', 'berl', 'richter') then */
         x_return :=  'ALLE_L_S';
         /*return x_return;
       end if;
       if lower(i_user) in ('harms','haas','sivas') then 
         x_return :=  'ABT_L_S';
         return x_return;
       end if;
       if lower(i_user) in ('meier', 'unger', 'bill', 'bartl', 'boehler') then 
         x_return :=  'SERVICE_L_S';
         return x_return;
       end if;
       if lower(i_user) in ('geiger') then 
         x_return :=  'PL_L_S';
         return x_return;
       end if;
       if x_return = 'N' then
         x_return := 'EIGENE_L_S';
         return x_return;
       end if;
       return x_return;*/
end;

------------------------------------------------------------------------
--######################################################################
------------------------------------------------------------------------

procedure a1_ma_anzeige(i_user varchar2) is

cursor c_ma_alle is
select t.persnr, t.familienname, t.vorname from PDE_PERSSTAMM t
where nvl(ausblenden_jn ,'N') = 'N'   
and (to_char (austrdat,'yyyy') >= to_char (sysdate, 'yyyy') or austrdat is null)
and (to_char (eintrdat,'YYYY') <= to_char (sysdate, 'yyyy'))
order by t.familienname;

xx_ma_alle c_ma_alle%rowtype;

begin
  
delete from sl_a1_antrag_ma;
commit;

--x_ls_jn := a1_lesen_schreiben_jn(i_user);

open c_ma_alle;
      loop
         fetch c_ma_alle into xx_ma_alle;
         exit when c_ma_alle%notfound;
         insert into sl_a1_antrag_ma values xx_ma_alle;
         commit;
       end loop;
    close c_ma_alle;
end;

------------------------------------------------------------------------
--######################################################################
------------------------------------------------------------------------

procedure a1_gepa_volltext(i_text in varchar2, i_fill_table_jn in varchar2) is
  
x_param1 varchar2(50);
x_param2 varchar2(50);
x_param3 varchar2(50);
x_param4 varchar2(50);
x_param5 varchar2(50);
x_param6 varchar2(50);
x_param7 varchar2(50);
x_param8 varchar2(50);



x_text varchar2(200);
x_param_count number(11);
x_zaehler number(11);

cursor c_param is
select * from sl_split_string;

x_param varchar2(50);

cursor c_gepa_count is
select  count(*)
  from GEPA t where t.gepa_c1 in('K', 'L', 'I') and t.land not in ('Deutschland') and t.kz_aktiv = 'J'
 and rowid in (select saq_GEPA.such_rowid
                   from saq_GEPA
                  where saq_GEPA.text_char like '%'||x_param1||'%' 
                    and saq_GEPA.text_char like '%'||x_param2||'%'
                    and saq_GEPA.text_char like '%'||x_param3||'%'
                    and saq_GEPA.text_char like '%'||x_param4||'%'
                    and saq_GEPA.text_char like '%'||x_param5||'%'
                    and saq_GEPA.text_char like '%'||x_param6||'%'
                    and saq_GEPA.text_char like '%'||x_param7||'%'
                    and saq_GEPA.text_char like '%'||x_param8||'%'
                    
                 UNION
                 select saq_GEPA.such_rowid
                   from saq_GEPA
                  where saq_GEPA.text like '%'||x_param1||'%'
                    and saq_GEPA.text like '%'||x_param2||'%'
                    and saq_GEPA.text like '%'||x_param3||'%'
                    and saq_GEPA.text like '%'||x_param4||'%'
                    and saq_GEPA.text like '%'||x_param5||'%'
                    and saq_GEPA.text like '%'||x_param6||'%'
                    and saq_GEPA.text like '%'||x_param7||'%'
                    and saq_GEPA.text like '%'||x_param8||'%'
                    );

x_gepa_count number(11);

cursor c_gepa is
select  t.kdnr, nvl(t.name1,t.name2) as "name" , t.strasse, t.ort, t.land, t.plz
  from GEPA t where t.gepa_c1 in ('K', 'L', 'I') and t.land not in ('Deutschland') and t.kz_aktiv = 'J'
 and rowid in (select saq_GEPA.such_rowid
                   from saq_GEPA
                  where saq_GEPA.text_char like '%'||x_param1||'%' 
                    and saq_GEPA.text_char like '%'||x_param2||'%'
                    and saq_GEPA.text_char like '%'||x_param3||'%'
                    and saq_GEPA.text_char like '%'||x_param4||'%'
                    and saq_GEPA.text_char like '%'||x_param5||'%'
                    and saq_GEPA.text_char like '%'||x_param6||'%'
                    and saq_GEPA.text_char like '%'||x_param7||'%'
                    and saq_GEPA.text_char like '%'||x_param8||'%'
                    
                 UNION
                 select saq_GEPA.such_rowid
                   from saq_GEPA
                  where saq_GEPA.text like '%'||x_param1||'%'
                    and saq_GEPA.text like '%'||x_param2||'%'
                    and saq_GEPA.text like '%'||x_param3||'%'
                    and saq_GEPA.text like '%'||x_param4||'%'
                    and saq_GEPA.text like '%'||x_param5||'%'
                    and saq_GEPA.text like '%'||x_param6||'%'
                    and saq_GEPA.text like '%'||x_param7||'%'
                    and saq_GEPA.text like '%'||x_param8||'%'
                    );

xx_gepa c_gepa%rowtype;

begin

delete from SL_A1_ANTRAG_GEPA;
commit;

x_param1 := '';
x_param2 := '';
x_param3 := '';
x_param4 := '';
x_param5 := '';
x_param6 := '';
x_param7 := '';
x_param8 := '';

x_zaehler := 1;

x_text := UPPER(i_text);
x_param_count := sl_allgemein.split_string(x_text, ' ');

if x_param_count > 0 then
  open c_param;
    loop
       fetch c_param into x_param;
       exit when c_param%notfound;
       
       if x_zaehler = 1 then
         x_param1 := x_param;
       elsif x_zaehler = 2 then
         x_param2 := x_param;
       elsif x_zaehler = 3 then
         x_param3 := x_param;
       elsif x_zaehler = 4 then
         x_param4 := x_param;
       elsif x_zaehler = 5 then
         x_param5 := x_param;
       elsif x_zaehler = 6 then
         x_param6 := x_param;
       elsif x_zaehler = 7 then
         x_param7 := x_param;
       elsif x_zaehler = 8 then
         x_param8 := x_param;
       end if;
       
       x_zaehler := x_zaehler +1;
     end loop;
  close c_param;       

open c_gepa_count;
     fetch c_gepa_count into x_gepa_count;
close c_gepa_count;

      if x_gepa_count > 0 then
            if i_fill_table_jn = 'J' then         
                   open c_gepa;
                     loop
                        fetch c_gepa into xx_gepa;
                        exit when c_gepa%notfound;
                        
                        insert into SL_A1_ANTRAG_GEPA t values xx_gepa;
                        commit;  
                     
                     end loop;
                   close c_gepa;
             end if;
         
      end if;        
end if;
end;

------------------------------------------------------------------------
--######################################################################
------------------------------------------------------------------------

procedure a1_gepa_ansprech(i_kdnr number) is

cursor c_anspr_count is  
select count(*) from GEPA_ANSPRECH_PARTNER t
where t.kdnr = i_kdnr;

x_count number(11);

cursor c_anspr is  
select t.kdnr, t.name1, t.name2 from GEPA_ANSPRECH_PARTNER t
where t.kdnr = i_kdnr;

xx_anspr c_anspr%rowtype;

begin

delete from sl_a1_antrag_anspr;
commit;

open c_anspr_count;
  fetch c_anspr_count into x_count;
close c_anspr_count;

if x_count > 0 then
  open c_anspr;
    loop
      fetch c_anspr into xx_anspr;
      exit when c_anspr%notfound;
           insert into sl_a1_antrag_anspr values xx_anspr;
           commit;     
    end loop;
  close c_anspr;
end if;
end;

procedure a1_anlegen(  i_pers_nr in number,
                             i_fam_name in varchar2,
                             i_name_vorname in varchar2,
                             i_von in varchar2,
                             i_bis in varchar2,
                             i_kdnr in number default null,
                             i_firma in varchar2,
                             i_strasse in varchar2,
                             i_plz in varchar2,
                             i_ort in varchar2,
                             i_land in varchar2,
                             i_angelegt_von in varchar2) is
                             
x_von date;
x_bis date;
x_angelegt_am date;

cursor c_lfdnr is
select max(t.lfdnr)+1 from sl_a1_antrag_tab t;

x_lfdnr number;

begin

x_von := trunc(to_date(i_von,'DD.MM.YY'));
x_bis := trunc(to_date(i_bis,'DD.MM.YY'));
x_angelegt_am := trunc(to_date(sysdate,'DD.MM.YY'));


open c_lfdnr;
     fetch c_lfdnr into x_lfdnr;
close c_lfdnr;

insert into sl_a1_antrag_tab(pers_nr,
                             fam_name,
                             name_vorname,
                             von,
                             bis,
                             kdnr,
                             firma,
                             strasse,
                             plz,
                             ort,
                             land,
                             angelegt_am,
                             angelegt_von,
                             beantragt_jn,
                             genehmigt_jn,
                             lfdnr,
                             status,
                             bearbeitet_am,
                             bearbeitet_von)
                      values(i_pers_nr,
                             i_fam_name,
                             i_name_vorname,
                             x_von,
                             x_bis,
                             i_kdnr,
                             replace(i_firma, '''',''),
                             replace(i_strasse, '''',''),
                             i_plz,
                             i_ort,
                             i_land,
                             x_angelegt_am,
                             i_angelegt_von,
                             'N',
                             'N',
                             x_lfdnr,
                             '20 in Bearbeitung Perso',
                             x_angelegt_am,
                             i_angelegt_von);
                             commit;
                             

      a1_workflow_status(i_lfdnr     => x_lfdnr,
                         i_oldstatus => '10 neuer Antrag',
                         i_newstatus => '20 in Bearbeitung Perso');          

end;


procedure a1_bearbeiten(     i_lfdnr in number,
                             i_von in varchar2,
                             i_bis in varchar2,
                             i_strasse in varchar2,
                             i_plz in varchar2,
                             i_ort in varchar2,
                             i_user in varchar2,
                             i_newstatus in varchar2) is


cursor c_status_old is
select a.status from sl_a1_antrag_tab a
where a.lfdnr = i_lfdnr;

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
        set a.von = x_von, a.bis = x_bis, a.strasse = i_strasse, a.plz = i_plz, a.ort = i_ort, a.bearbeitet_am = trunc(sysdate),
        a.bearbeitet_von = i_user, a.status = i_newstatus 
        where a.lfdnr = i_lfdnr;
        commit;
        
              a1_workflow_status(i_lfdnr     => i_lfdnr,
                         i_oldstatus => x_status_old,
                         i_newstatus => i_newstatus); 
        
end;



procedure update_delete(i_lfdnr in number, i_function in varchar2, i_user in varchar2) is

cursor c_status_old is
select a.status from sl_a1_antrag_tab a
where a.lfdnr = i_lfdnr;

x_user varchar(200);
x_status_new varchar2(255);
x_status_old varchar2(255);


begin

/*10 ¿ in Arbeit
20 ¿ in Bearbeitung Perso
30 ¿ zur Beantragung
40 ¿ vorläufige Bescheinigung erhalten
50 ¿ Bescheinigung erhalten
60 - Bestehende Bescheinigung bearbeitet
80 - geändert
85 - Antrag Stornierung Mitarbeiter
90 - Antrag ungültig*/

if i_user is null then
  x_user := 'SIVAS';
else
  x_user := i_user;
end if;

open c_status_old;
  fetch c_status_old into x_status_old;
close c_status_old;

      if i_function = '50' then
        x_status_new := '50 Bescheinigung erhalten';
        update sl_a1_antrag_tab a
        set a.genehmigt_am = trunc(sysdate), a.genehmigt_von = x_user, a.status = x_status_new,
        a.bearbeitet_von = x_user, a.bearbeitet_am = trunc(sysdate)
        where a.lfdnr = i_lfdnr;
        commit;
     
      elsif i_function = '40' then
        x_status_new := '40 vorläufige Bescheinigung erhalten';
        update sl_a1_antrag_tab a
        set a.VORL_ERH_am = trunc(sysdate), a.VORL_ERH_von = x_user, a.status = x_status_new,
        a.bearbeitet_von = x_user, a.bearbeitet_am = trunc(sysdate)
        where a.lfdnr = i_lfdnr;
        commit;
        
      elsif i_function = '30' then
        x_status_new := '30 zur Beantragung an Krankenkasse';
        update sl_a1_antrag_tab a
        set a.beantragt_am = trunc(sysdate), a.beantragt_von = x_user, a.status = x_status_new,
        a.bearbeitet_von = x_user, a.bearbeitet_am = trunc(sysdate)
        where a.lfdnr = i_lfdnr;
        commit;
        
      elsif i_function = '20' then
        x_status_new := '20 in Bearbeitung Perso';
        update sl_a1_antrag_tab a
        set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
        where a.lfdnr = i_lfdnr;
        commit;
        
     elsif i_function = '10' then
        x_status_new := '10 in Arbeit';
        update sl_a1_antrag_tab a
        set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
        where a.lfdnr = i_lfdnr;
        commit;
        
      elsif i_function = '90' then
        x_status_new := '90 Antrag ungültig (löschen)';
        update sl_a1_antrag_tab a
        set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
        where a.lfdnr = i_lfdnr;
        commit;
        
      elsif i_function = '80' then
        x_status_new := '80 geändert';
        update sl_a1_antrag_tab a
        set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
        where a.lfdnr = i_lfdnr;
        commit;
      
      elsif i_function = '85' then
        x_status_new := '85 Stornierung beantragt';
        update sl_a1_antrag_tab a
        set a.bearbeitet_am = trunc(sysdate), a.bearbeitet_von = x_user, a.status = x_status_new
        where a.lfdnr = i_lfdnr;
        commit;
      end if;
      
      a1_workflow_status(i_lfdnr     => i_lfdnr,
                         i_oldstatus => x_status_old,
                         i_newstatus => x_status_new);
      
end;

procedure a1_workflow_status(i_lfdnr in number, i_oldstatus in varchar2, i_newstatus in varchar2) is

cursor c_antrag is
select * from sl_a1_antrag_tab a
where a.lfdnr = i_lfdnr;

xx_antrag c_antrag%rowtype;

x_pers number;
x_user varchar2(100);

cursor c_pde_user is
select t.e_mail from pde_persstamm t
where nvl(ausblenden_jn ,'N') = 'N'   
and (to_char (austrdat,'yyyy') >= to_char (sysdate, 'yyyy') or austrdat is null)
and (to_char (eintrdat,'YYYY') <= to_char (sysdate, 'yyyy')) 
and lower(t.login_user) = lower(x_user);

cursor c_pde_persnr is
select t.e_mail from pde_persstamm t
where nvl(ausblenden_jn ,'N') = 'N'   
and (to_char (austrdat,'yyyy') >= to_char (sysdate, 'yyyy') or austrdat is null)
and (to_char (eintrdat,'YYYY') <= to_char (sysdate, 'yyyy')) 
and t.persnr = x_pers;

cursor c_pde_angelegt_von is
select t.e_mail from pde_persstamm t
where nvl(ausblenden_jn ,'N') = 'N'   
and (to_char (austrdat,'yyyy') >= to_char (sysdate, 'yyyy') or austrdat is null)
and (to_char (eintrdat,'YYYY') <= to_char (sysdate, 'yyyy')) 
and lower(t.login_user) = lower(x_user);

x_mail_body varchar2(1000);


x_send_to varchar2(100);
x_send_from varchar2(100);

begin

if i_oldstatus != i_newstatus then

open c_antrag;
     fetch c_antrag into xx_antrag;
close c_antrag;
      
      -- Neuer Antrag --> 20
      -- Bestehender Antrag geändert --> 60
      -- Antrag geändert --> 80
      -- Antragsteller an PZE 
      if substr(i_newstatus,1,2) = '20' or substr(i_newstatus,1,2) = '60' or substr(i_newstatus,1,2) = '80'  then
       
       x_user := xx_antrag.angelegt_von; 
       open c_pde_user;
         fetch c_pde_user into x_send_from;
       close c_pde_user;
            
        x_send_to := 'pv@servolift.de';
        x_mail_body := 'A1 Antrag angelegt bzw. geändert' || chr(13)|| chr(13) ||
                       'LFDNR: ' || xx_antrag.lfdnr || chr(13) ||
                       'Mitarbeiter: ' || xx_antrag.pers_nr  ||', '|| xx_antrag.fam_name ||', '|| xx_antrag.name_vorname || chr(13) ||
                       'VON: ' || xx_antrag.von ||', BIS: ' || xx_antrag.bis || chr(13) ||
                       'Firma: ' || xx_antrag.firma || ', Land: ' || xx_antrag.land || chr(13) ||                     
                       'Angelegt am: ' || xx_antrag.angelegt_am || ', von: ' || xx_antrag.angelegt_von|| chr(13)|| chr(13) ||
                       'Status alt: '|| i_oldstatus || chr(13)|| 'Status neu: ' || i_newstatus; 
                        
              if x_send_from is not null and x_send_to is not null then
                  sendmail_smtp(SendTo => x_send_to,
                                SendFrom => x_send_from,
                                MailSubject => 'A1 Antrag angelegt bzw. geändert',
                                MailBody => x_mail_body,
                                SmtpHost => 'slex' );
                  
                  
/*                  sendmail_smtp(SendTo => 'haas@servolift.de',
                                SendFrom => x_send_from,
                                MailSubject => 'A1 Antrag angelegt bzw. geändert',
                                MailBody => x_mail_body,
                                SmtpHost => 'slex' );*/
              end if;
              
       end if;       
      -- Antrag storniert von Mitarbeiter --> 85
      -- Antragsteller an PZE 
      if substr(i_newstatus,1,2) = '85' then
       
       x_user := xx_antrag.angelegt_von; 
       open c_pde_user;
         fetch c_pde_user into x_send_from;
       close c_pde_user;
            
        x_send_to := 'pv@servolift.de';
        x_mail_body := 'A1 Antrag - Stornierung von Mitarbeiter beantragt' || chr(13)|| chr(13) ||
                       'LFDNR: ' || xx_antrag.lfdnr || chr(13) ||
                       'Mitarbeiter: ' || xx_antrag.pers_nr  ||', '|| xx_antrag.fam_name ||', '|| xx_antrag.name_vorname || chr(13) ||
                       'VON: ' || xx_antrag.von ||', BIS: ' || xx_antrag.bis || chr(13) ||
                       'Firma: ' || xx_antrag.firma || ', Land: ' || xx_antrag.land || chr(13) ||                     
                       'Angelegt am: ' || xx_antrag.angelegt_am || ', von: ' || xx_antrag.angelegt_von|| chr(13)|| chr(13) ||
                       'Status alt: '|| i_oldstatus || chr(13)|| 'Status neu: ' || i_newstatus; 
                        
              if x_send_from is not null and x_send_to is not null then
                  sendmail_smtp(SendTo => x_send_to,
                                SendFrom => x_send_from,
                                MailSubject => 'A1 Antrag Stornierung',
                                MailBody => x_mail_body,
                                SmtpHost => 'slex' );
                  
                  
/*                  sendmail_smtp(SendTo => 'haas@servolift.de',
                                SendFrom => x_send_from,
                                MailSubject => 'A1 Antrag Stornierung',
                                MailBody => x_mail_body,
                                SmtpHost => 'slex' );*/
              end if;
              
       end if;       
      -- vorläufige Genehmigung erhalten --> 40 oder --> 50 oder --> 30
      -- PZE an Antragsteller
      if substr(i_newstatus,1,2) = '50' or substr(i_newstatus,1,2) = '40' or substr(i_newstatus,1,2) = '30' then
               x_user := xx_antrag.angelegt_von; 
               open c_pde_angelegt_von;
                 fetch c_pde_angelegt_von into x_send_to;
               close c_pde_angelegt_von;
                    
                x_send_from := 'pv@servolift.de';
                x_mail_body := 'A1 Antrag Statusänderung' || chr(13)|| chr(13) ||
                       'LFDNR: ' || xx_antrag.lfdnr || chr(13) ||
                       'Mitarbeiter: ' || xx_antrag.pers_nr  ||', '|| xx_antrag.fam_name ||', '|| xx_antrag.name_vorname || chr(13) ||
                       'VON: ' || xx_antrag.von ||', BIS: ' || xx_antrag.bis || chr(13) ||
                       'Firma: ' || xx_antrag.firma || ', Land: ' || xx_antrag.land || chr(13) ||                     
                       'Angelegt am: ' || xx_antrag.angelegt_am || ', von: ' || xx_antrag.angelegt_von|| chr(13)|| chr(13) ||
                       'Status alt: '|| i_oldstatus || chr(13)|| 'Status neu: ' || i_newstatus; 
                        
                        if x_send_from is not null and x_send_to is not null then
                            sendmail_smtp(SendTo => x_send_to,
                                          SendFrom => x_send_from,
                                          MailSubject => 'A1 Antrag Statusänderung',
                                          MailBody => x_mail_body,
                                          SmtpHost => 'slex' );
                                          
/*                            sendmail_smtp(SendTo => 'haas@servolift.de',
                                          SendFrom => x_send_from,
                                          MailSubject => 'A1 Antrag Statusänderung',
                                          MailBody => x_mail_body,
                                          SmtpHost => 'slex' );*/
                      end if;
      
      end if;
end if;

end;
end SL_A1_ANTRAG;
```
