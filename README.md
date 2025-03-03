# Airport App API

**Airport App API** to RESTful API stworzone w .NET, umożliwiające zarządzanie rezerwacjami lotniczymi.

## 📌 Funkcjonalności

- Zarządzanie lotniskami, lotami i rezerwacjami
- Obsługa CRUD dla głównych zasobów
- Autoryzacja i uwierzytelnianie użytkowników
- Walidacja danych wejściowych

## 🛠️ Wymagania

- .NET 5.0 lub nowszy

## 🚀 Instalacja

1. **Sklonuj repozytorium**

   ```bash
   git clone https://github.com/JakubTeichman/Airport_App_API.git
   cd Airport_App_API
   ```

2. **Zainstaluj zależności**

   ```bash
   dotnet restore
   ```

3. **Uruchom aplikację**

   ```bash
   dotnet run
   ```

   Aplikacja będzie dostępna pod adresem `https://localhost:5001`.

## 🔗 Endpointy API

- `GET /api/airports` - Pobiera listę lotnisk
- `POST /api/airports` - Dodaje nowe lotnisko
- `GET /api/flights` - Pobiera listę lotów
- `POST /api/flights` - Dodaje nowy lot
- `GET /api/reservations` - Pobiera listę rezerwacji
- `POST /api/reservations` - Tworzy nową rezerwację

## 🧪 Testowanie API

Aby przetestować API, zaleca się użycie Swagger UI. Po uruchomieniu aplikacji przejdź do:

```
https://localhost:5001/swagger
```

Swagger umożliwia interaktywne testowanie endpointów oraz podgląd dokumentacji API.

## 📜 Licencja

Projekt jest udostępniony na licencji MIT.

---
Jeśli masz pytania lub sugestie, zapraszam do kontaktu! ✈️

