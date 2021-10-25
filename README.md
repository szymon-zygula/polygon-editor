# Polygon Editor

Aby utworzyć wielokąt należy nacisnąć przycisk "Draw polygon".
Kliknięcie lewym przyciskiem myszy doda wierzchołek, prawym zamknie wielokąt i zakończy rysowanie.

Aby utworzyć okrąg należy nacisnąć przycisk "Draw circle".
Kliknięcie lewym przyciskiem myszy ustala środek okręgu, prawym kończy rysowanie, promień ustalany jest przesuwaniem myszy.

Aby edytować okrąg/wielokąt należy wybrać go z listy.
Można wtedy go przesuwać (chwytając różowy kwadrat w wielokącie/okręgu),
  przesuwać krawędzie i wierzchołki (chwytając odpowiednie pola)
  i zmieniać promień okręgu chwytając kółko na jego brzegu.
  
Aby dodać wierzchołek na krawędzi należy nacisnąć na nią kółkiem myszy.
Aby usunąć wierzchołek należy nacisnąć na niego prawym przyciskiem myszy.
Aby usunąć wielokąt/okrąg należy nacisnąć na różowy kwadrat w jego środku prawym przyciskiem myszy.

Aby dodać relację należy nacisnąć na odpowiedni przycisk, a następnie wybrać krawędzie/okręgi.
Ograniczenie "zadana długość krawędzi" i "zadany promień okręgu" wymagają wpisania w pole tekstowe oczekiwanej wartości.

Ograniczenia obliczane są rekurencyjnie: jeśli zmieni się położenie jakiegoś wierzchołka,
  to modyfikowane są pozycje wierzchołków sąsiednich ograniczeń tak,
  by te były spełnione. Następnie kolejnych itd.
W kodzie ustawiony jest limit głębokości takiej rekurencji i dozwolona niedokładność.
Pozwala to na zbieżność kształtów do zadanych przez ograniczenia pozycji bez definiowania explicite przypadków brzegowych nawet w skomplikowanych ustawieniach.
