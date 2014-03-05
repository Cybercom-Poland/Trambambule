<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="Trambambule.Help" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>POMOC</h1>

    <div class="ContentPanel" style="padding: 10px; line-height: 20px;">
        
        <h2>O TRAMBAMBULACH</h2>
        <p>Aplikacja Trambambule służy do wyznaczania rankingu wewnętrznego w piłkarzyki w naszej firmie. </p>
        <p>Obecnie uwzględnia wszystkich pracowników z Warszawy, docelowo rozszerzona zostanie o pracowników z Łodzi.</p>
        <p>Aby dodać nowego gracza bądź usunąć nieaktywnego, skontaktuj się z Mariuszem Milewskim.</p>
        <h2>INSTRUKCJA ZGŁASZANIA WYNIKÓW MECZY</h2>
        <p>Po każdym rozegranym meczu, wyznacz jedną osobę do wprowadzenia wyników do systemu.</p>
        <p>1. Aby zgłociś wynik rozegranego meczu, udaj się na stronę <a href="SendResult.aspx">Zgłoś wynik meczu</a></p>
        <p>2. Wprowadź nazwiska graczy w odpowiednie pola: Drużyna 1 Atak/Obrona, Drużyna 2 Atak/Obrona. Każde wprowadzane
        nazwisko musi uprzednio istnieć w systemie i pojawić się w liście podpowiedzi po wpisaniu min. 2 znaków z imienia bądź nazwiska.
        W przypadku gdy podany tekst nie odpowiada imieniu i nazwisku gracza istniejącego w bazie, tekst jest czyszczony i wymagane jest
        wprowadzenie ponowne poprawionych danych.</p>
        <p>3. Wprowadź wynik w bramkach. W pola wyniku możliwe jest wpisywanie wyłącznie cyfr. Możliwe jest wprowadzanie dowolnych wyników 
        (nawet remisów). Mimo to przyjmujemy, że rozgrywamy mecze do 10 dla jednej z drużyn.</p>
        <p>4. W przypadku błędnego wprowadzenia meczu, wyślij informację z prośbą o usunięcie meczu z bazy do Mariusza Milewskiego z informacjami:
        gracze którzy wzięli wzięli udział w meczu, wynik, data wprowadzenia meczu z godziną co do minuty.</p>
        <p>5. Statystyki graczy oraz ranking zostaną automatycznie zaktualizowane, a pole wyniku wyczyszczone.</p>
        <p>6. W przypadku wprowadzania kilku meczy pod rząd z rotowanym składem, można odwrócić role w dróżynie (Atak/Obrona) przyciskiem rotowanych
        strzałek.</p>
        <p>7. Aby wyczyścić wszystkie pola ewidencji meczu wciśnij przycisk "Wyczyść".</p>
        <h2>KORZYSTANIE Z APLIKACJI</h2>
        <p>Aplikacja poza zgłaszaniem meczy umożliwia monitorowanie <a href="/LastGames.aspx">historii rozegranych gier</a> przez konkretnego gracza oraz podgląd globalnego
        <a href="/Default.aspx">rankingu firmowego</a>.        </p>
        <p>Na pasku bocznym po prawej stronie wyświetlane są szczegółowe statytyki wybranego gracza. Aby wybrać gracza należy wprowadzić jego
        imię i nazwisko (podobnie jak przy wprowadzaniu wyniku) w polu tekstowym u góry prawego paska. Po wybraniu gracza, wszystkie statystyki
        na poszczególnych podstronach również będą spersonalizowane dla wybranego gracza.    </p>
        <h2>JAK DZIAŁA RANKING?</h2>
            <p>Ranking składa się z dwóch liczb: punktów, które widoczne są na stronie oraz niepewności, która określa przedział, w jakim znajduje się prawdziwy ranking z dużym prawdopodobieństwem.</p>

            <p>Początkową wartością punktową dla każdego gracza jest 1500 i może ona maleć lub rosnąć w zależności od procentowej ilości bramek strzelonych w każdym meczu.</p>

            <p>Niepewność na początku określona jest na 350, co oznacza że prawdziwy ranking gracza znajduje się w przedziale &lt;800,2200&gt; 
            (ze wzoru &lt;punkty-2*niepewność,punkty+2*niepewność&gt;) z prawdopodobieństwem 95%. Po każdym meczu wartość niepewności maleje, a 
            po dłuższej nieaktywności rośnie.</p>

            <p>Przy dużej niepewności rankingu gracza, jego widoczna wartość punktowa może zmieniać się o duże wartości. Po osiągnięciu stabilnego rankingu, zmiany nie będą tak drastyczne.</p>

            <p>Nowy ranking gracza określa wiele czynników:</p>

            <ul>
            <li>jego poprzedni ranking (obviously)</li>
            <li>rankingi przeciwników z momentu przed meczem</li>
            <li>ranking partnera (z wartością ujemną: im lepszy partner, tym oczekiwany wynik bramkowy będzie wyższy)</li>
            <li>wynik meczu w postaci procentowej ilości zdobytych bramek</li>
            </ul><p>To oznacza, że wynik 3-0 (przy grze na czas) i 10-0 dają tyle samo punktów rankingowych, a 10-9 (zwycięstwo) i 9-10 (przegrana) nie będą zmieniały rankingów graczy w sposób zdecydowanie różny.<br>
            Warto zaznaczyć, że jeśli dwóch graczy o wysokim rankingu będzie grało z dwoma o bardzo niskim, ich wygrana może przynieść im spadek punktów rankingowych, jeśli nie wygrają do zera. Nawet jedna stracona bramka może oznaczać, że nie są aż tak dobrzy, jakby wskazywał na to ich aktualny ranking.<br>
            Z tego powodu najlepiej grać w parach, których sumy rankingów są zbliżone.</p>

            <p>Więcej szczegółów można znaleźć na <a href="http://www.glicko.net/glicko.html">stronie Marka Glickmana</a>, a w szczególności w jego <a href="http://www.glicko.net/glicko/glicko.pdf">opisie rankingu Glicko</a>.</p>

        <span class="center">
            W razie pytań odnośnie działania aplikacji, sugestii, etc. proszę kontaktować się z Mariuszem Milewskim.
            <br />
            W razie pytań odnośnie wyznaczania punktów rankingowych, proszę kontaktować się z Maćkiem Górskim.   
            <br />
            <br />
        </span>
    </div>

</asp:Content>
