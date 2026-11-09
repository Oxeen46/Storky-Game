# Storky çalışma rehberi

1. [Çalışma günlüğünün üst bölümünü](docs/CALISMA_GUNLUGU.md#guncel-durum) oku. Güncel durum, karar tablosu, sıradaki işler ve dosya haritası devir teslim için başlangıç noktasıdır. Eski oturumlara gerekçe veya test sonucu gerektiğinde bak.
2. Kurulum ve kontroller için [README](README.md), tasarım için [GDD](docs/Storky_GDD.pdf) kullan. Kullanıcının son talimatı önceliklidir. GDD hedeflenen oyunu, kayıtlı sahne ve kod uygulanmış davranışı gösterir. Çelişki varsa belirt.
3. Değişiklikten önce Git durumunu ve ilgili kod, sahne, Prefab veya sanat kaynağını incele. Mevcut kullanıcı değişikliklerini koru. Yalnızca GitHub bağlantısına erişebiliyorsan yayımlanan sürüm üzerinden konuş.
4. Unity projesi `UnityProject/`, oyuna özgü varlıklar `UnityProject/Assets/_Game/`, düzenlenebilir Blender kaynakları `ArtSource/` altında. Unity sürümünü [ProjectVersion.txt](UnityProject/ProjectSettings/ProjectVersion.txt), paketleri [manifest](UnityProject/Packages/manifest.json) ve kilit dosyasından doğrula. Sürüm değişikliği için kullanıcıyla kapsamı netleştir.
5. Denemeye kayıtlı [FlightTest](UnityProject/Assets/_Game/Scenes/FlightTest.unity) sahnesinden başla. Sahne kurulum ve hareket kalibrasyon betiklerini ancak ilgili değişiklik gerektiğinde, etkilerini okuyarak çalıştır. Önce sahneyi ve araç bağlantısını doğrula.
6. Unity varlıklarıyla `.meta` dosyalarını birlikte taşı. Git LFS kurallarını koru. Library, Temp, derleme çıktıları, günlükler ve Blender yedekleri depoya girmez.
7. Commit ve push işlemlerini kullanıcı yapar. Uzak depo ayarları veya geçmişi için açık talimatı esas al.
8. Oturum numarasını günlüğün üstünden al. Aynı iş yeni sohbette de aynı oturumda sürer. Kullanıcı yeni oturum açarsa verdiği numarayı, numara vermediyse sıradaki gerçek numarayı kullan. İstendiğinde sohbet adı `Unity-Storky-N` biçimindedir.
9. Günlük kullanıcı istediğinde veya onayladığında güncellenir. Üstteki durum ve karar tablosunu eşleştir, değişen kararın gerekçesini ilgili oturuma kısa kaydet ve oturumları tarihsiz tut. Doğrulanan sonuçları, kullanıcı geri bildirimini ve bekleyen testleri ayır.
10. Sonucu Türkçe özetle. Neyi değiştirdiğini, nasıl kontrol ettiğini ve varsa bir sonraki adımı yaz.
