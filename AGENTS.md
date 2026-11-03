# Ajan çalışma yönergeleri

## Başlamadan önce

- `README.md` ve `docs/CALISMA_GUNLUGU.md` dosyalarını oku. Günlüğün başındaki güncel durum ile son oturumdan devam et.
- Git durumunu ve ilgili dosyaları kontrol et; kullanıcıya ait mevcut değişiklikleri koru.
- Kullanıcının güncel talimatları bu yönergelerden önceliklidir.

## Çalışma sınırları

- Unity projesi `UnityProject/`, düzenlenebilir sanat kaynakları `ArtSource/` içindedir.
- Unity sürümü için `UnityProject/ProjectSettings/ProjectVersion.txt`, paketler için manifest ve kilit dosyası esastır.
- `.meta` dosyalarını varlıklardan ayırma. LFS kurallarını ve `.gitignore` dosyalarını koru.
- Gereksiz Library, Temp, günlük, otomatik yedek veya derleme çıktılarını ekleme.
- Kullanıcı yetkilendirmeden commit, push, uzak repo ayarı değişikliği veya geçmişi yeniden yazma işlemi yapma. Yetki verilmişse aynı işlem için tekrar tekrar onay isteme.
- Önceki oturumda verilmiş belirli bir işlem yetkisini tüm gelecek gönderimler için sınırsız yetki sayma.

## Her çalışma sonunda

- İş ve karar geçmişini tek yerde, `docs/CALISMA_GUNLUGU.md` içinde tut. Ayrı karar/günlük dosyaları açarak geçmişi dağıtma.
- **Tarih yazma.** Oturumları `Oturum 1`, `Oturum 2` şeklinde artan sıra ile dosyanın sonuna ekle.
- Aynı kesintisiz çalışmadaki düzeltmeleri aynı oturumda güncelle; her araç çağrısına oturum açma.
- Amaç, yapılanlar, karar ve gerekçeler, değişen önemli dosyalar, kontroller, eksikler, devam noktası ve Git kaydını doldur.
- Günlüğün başındaki güncel durum ve sıradaki işleri güncelle. Geçmiş karar değişirse eskisini silme; yeni oturumda önceki karara bağlantı ver.
- Doğruladığın sonuçları, kullanıcı bildirimlerini ve önceki konuşmadan aktarılan bilgileri ayır. Planı tamamlanmış iş; yapılmayan testi başarılı test gibi yazma.
- Commit yapılmadıysa `Commit/push yapılmadı` yaz. Bir commit kendi kimliğini içeremez; ilgili kimliği sonraki oturumda ekle veya commit başlığıyla tanımla. Sadece günlükte hash yazmak için gereksiz yeni commit oluşturma.
- Belgeler için bağlantı/içerik kontrolleri yeterlidir; gereksiz Unity veya Blender çalıştırma. Ürün değişikliklerinde ilgili kontrolleri yap ve sınırlarını yaz.

## Kullanıcıya dönüş

Türkçe, anlaşılır biçimde ne değiştiğini, nasıl kontrol edildiğini ve neyin kaldığını bildir. Dosyalara bağlantı ver. Commit/push durumunu açık belirt.
