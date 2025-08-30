# CustomerSegmentation-RFM-ConsoleApp

.NET Framework C# Konsol Uygulaması ile **Müşteri Segmentasyonu (RFM Analizi)**

## 📌 Açıklama
Bu uygulama, müşteri verilerini analiz ederek onları farklı segmentlere ayırır. RFM modeli (Recency, Frequency, Monetary) kullanarak her müşteriye skor hesaplar ve segment atar. CSV dosyası ile veri yükleyebilir veya örnek verilerle çalışabilirsiniz. Sonuçları konsolda görebilir ve CSV’ye dışa aktarabilirsiniz.

## 🚀 Özellikler
- Örnek veri seti ile çalışabilme
- CSV dosyasından müşteri verilerini yükleyebilme
- Her müşteri için RFM skoru hesaplama
- Segment atama: Champions, Loyal, New Customer, Hibernating, Others
- Konsolda tablo ve segment bazlı özet rapor
- En değerli 3 müşteriyi listeleme
- Segment filtreleme
- Sonuçları CSV’ye dışa aktarma

## 🛠️ Kullanım
1. Projeyi Visual Studio ile açın.
2. Uygulamayı çalıştırın.
3. Örnek verilerle veya CSV dosyası ile analiz yapın.
4. Konsolda tablo ve segment özetini görüntüleyin.
5. CSV dosyasına dışa aktarılan sonuçları inceleyin.

## 📂 CSV Formatı
CSV dosyası aşağıdaki kolonları içermelidir:
Id,Name,LastPurchaseDate,PurchaseCount,TotalSpent

markdown
Kodu kopyala

- `LastPurchaseDate` formatı: `yyyy-MM-dd`
- Örnek:
1,Ayşe,2025-08-25,12,2450
2,Mehmet,2025-07-30,5,630

markdown
Kodu kopyala

## 📈 Segmentler
- **Champions:** Son zamanlarda ve sık alışveriş yapan, yüksek harcama yapan müşteriler
- **Loyal:** Düzenli alışveriş yapan müşteriler
- **New Customer:** Yeni müşteri
- **Hibernating:** Uzun süredir alışveriş yapmayan, düşük harcama yapan müşteriler
- **Others:** Diğer müşteriler

## 💻 Gereksinimler
- .NET Framework (4.x)
- Visual Studio 2019 veya üzeri

## 📌 Lisans
MIT License
