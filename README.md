# refactoring-renewal-example

W tym zadaniu Waszym celem jest refaktoryzacja kodu aplikacji nazwanej `LegacyRenewalApp`.

Zakładamy, że aplikacja działa poprawnie biznesowo i nie poprawiacie jej funkcjonalności. Waszym zadaniem jest wyłącznie poprawa jakości kodu.

Projekt `LegacyRenewalAppConsumer` pełni rolę kodu klienckiego. Tego projektu nie wolno modyfikować. Kod w projekcie `LegacyRenewalApp` może być modyfikowany tak długo, jak długo nie psujecie publicznego kontraktu wykorzystywanego przez `LegacyRenewalAppConsumer`.

Dodatkowo nie wolno modyfikować klasy `LegacyBillingGateway`. Zakładamy, że jest to zewnętrzna, statyczna klasa pochodząca ze starej biblioteki. Jeśli chcecie ograniczyć sprzężenie z nią, musicie ją opakować we własną abstrakcję.

## Cel zadania

To zadanie ma sprawdzić zrozumienie:

- DRY,
- zasad SOLID,
- kohezji,
- coupling,
- bezpiecznego refaktoryzowania działającego kodu.

## Co jest problemem w obecnym kodzie

Klasa `SubscriptionRenewalService` zawiera jedną długą metodę, która miesza wiele odpowiedzialności:

- walidację danych wejściowych,
- pobieranie danych z repozytoriów,
- logikę biznesową związaną z rabatami,
- logikę opłat dodatkowych,
- logikę podatkową,
- budowanie obiektu faktury,
- zapis faktury,
- wysyłkę wiadomości e-mail.

Dodatkowo w kodzie występują:

- rozbudowane bloki `if-else`,
- bezpośrednie tworzenie zależności wewnątrz serwisu,
- bezpośrednie użycie statycznej klasy zewnętrznej,
- logika, którą można później przenieść do osobnych klas lub serwisów domenowych.

## Oczekiwania wobec refaktoryzacji

Student powinien:

- ładnie podzielić kod na odpowiedzialności,
- poprawić kohezję klas,
- zmniejszyć coupling,
- usunąć powtórzenia,
- wydzielić fragmenty logiki do osobnych klas,
- rozważyć rozbicie części `if-else` na interfejsy i implementacje zgodnie z Open/Closed Principle,
- ograniczyć bezpośrednie zależności przez wprowadzenie abstrakcji i prostego IoC przez wstrzykiwanie zależności,
- opakować `LegacyBillingGateway` we własną klasę lub interfejs bez modyfikowania samej klasy statycznej.

## Ważne ograniczenia

- Nie zmieniaj kodu w `LegacyRenewalAppConsumer`.
- Nie zmieniaj klasy `LegacyBillingGateway`.
- Zachowaj publiczny kontrakt wykorzystywany przez kod kliencki.
- Zakładamy, że obecna logika działa poprawnie. Refaktoryzacja ma poprawić jakość kodu, a nie zmienić wynik działania z perspektywy klienta.
