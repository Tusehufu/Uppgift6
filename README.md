# Uppgift6
# Requirements 
Node.js
Vue CLI


### Installation av vue cli:
Jobba kommandot i kommandotolken.
npm install -g @vue/cli
npm install
### Hur man startar projektet
Högerklicka på mappen clientapp i visual studio och öppna den i file explorer. Töm sedan sökvägsfältet och skriv in "cmd ."
I kommandotolken som öppnas skriver du in "npm run serve". Då byggs programmet.
Sedan trycker du på kör programmet knappen i visual studio, välj http. 

När programmet körs öppnas API sidan på localhost/7056, ändra adressen till det som visas i kommandotolken från clientmappen. Förmodligen http://localhost:8080/.

### Applikationens utformning
Applikationen är utformad en en MVC model. Arkitekturen på applikationen är lagerbaserad och det finns en separering av ansvar för olika delar av applikationen i controllers, services och repositories. Controllern fungerar som ett gränssnitt mellan användaren och applikationen och tar emot alla HTTP-förfrågningar. Repositoryt ansvarar för att kommunicera med databasen och används med hjälp av dapper i applikationen för att exekvera SQL-frågor. Servicet innehåller affärslogik och fungerar som ett mellanlaget mellan repositoryt och kontrollern. Detta underlättar skalbarheten, delar av koden blir mer återanvändbar, underhållsarbete blir enklare och strukturen blir tydligare.

Applikationen använder sig av models samt viewmodels. Detta för att separera ansvar för att anpassa vilken data som ska visas eller tas emot av användargränssnittet. Detta minskar risken att känslig data exponeras. Det förbättrar även kodens läsbarhet och underhålllbarhet. 

Applikationen är dessutom uppdelad i 2 separata projekt som heter .App och .Infrastructure. Anledningen är för att skapa en mer modulär och underhållbar kodbad. Det innebär att ansvaret separeras. Services och repositories finns i infrastructure eftersom den hanterar all infrastrukturrelaterad logik. controllers finns i app projektet eftersom den hanterar användarinteraktioner och presenterar data. Fördelen med detta är att det blir en tydlig gränsdragning mellan presentation och affärslogik/dataåtkomst. En nackdeln är att infrastructure både innehåller affärslogik och dataåtkomstlogik.

Det finns en del kvar att göra på applikationen som inte hunnits.
Exempelvis radera postreplys, sätta relationen till en nyskapad reply till en post, layoutsaker etc.