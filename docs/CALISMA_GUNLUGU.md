# Storky — Çalışma günlüğü

İşler ve kararlar **tarih kullanılmadan, oturum sırasıyla** tek dosyada tutulur. Yeni oturum dosyanın sonuna eklenir. Bu ilk geçmiş özeti, önceki konuşma ve mevcut Git kayıtlarından yeniden oluşturuldu; birebir tüm eski çalışma seanslarının dökümü değildir.

## Güncel durum

| Konu | Durum ve dayanak |
| --- | --- |
| Repo | Storky-Game; bu incelemede yerel dal `main` |
| Kaynak model | `ArtSource/Stork/Stork_Hero_Lite.blend` mevcut; LFS ile takip ediliyor |
| Unity | 6000.3.27f1, URP 17.3.0; proje dosyalarından doğrulandı |
| Git ayarları | `.blend` ve `.fbx` LFS kuralları mevcut; iki seviyeli `.gitignore` düzeni var |
| Unity kayıt ayarları | Force Text ve Visible Meta Files proje dosyalarından doğrulandı |
| Sahne | `Assets/Scenes/SampleScene.unity`; şablon sahnesi |
| Oynanış | Henüz uygulanmadı |
| Leylek Unity aktarımı | Assets altında FBX bulunmadı; aktarım/Animator kurulumu doğrulanmadı |
| Belgeler | README, ajan yönergeleri ve bu günlük yerelde hazırlandı; commit/push yapılmadı |

### Sıradaki işler

1. Kullanıcıyla belge şablonunu gözden geçir; oyun fikri ve hedef platform gibi boş alanları doldur.
2. Renkli README şablonunu gözden geçir; şimdilik kapak, karakter görseli veya GIF ekleme.
3. Yetki verildiğinde belge değişikliklerini commit ve push yap.
4. Kullanıcının planına göre Unity klasör düzeni, ilk oyun sahnesi ve leyleğin aktarımına geç.
5. Ekipten başka bilgisayarda açılışı doğrula; henüz yapılmış sayma.

### Açık kararlar

- Oyuncunun temel amacı ve oyun döngüsü.
- Hedef platformlar ve performans hedefleri.
- Ekip görevleri, dal/inceleme düzeni ve dağıtım/lisans koşulları.
- README metin alanlarının kullanıcıyla doldurulması; mevcut tercih görselsiz renkli şablon.

---

## Oturum 1 — Repo ve büyük dosya takibi

### Amaç
Ortak GitHub reposunu ve sanat dosyaları için sürüm kontrolünü hazırlamak.

### Yapılanlar
- Kullanıcı Git ve GitHub Desktop kurulumunu tamamladı; Storky-Game reposunu yerel bilgisayara aldı.
- Kullanıcı commit kimliği için GitHub noreply adresini seçtiğini bildirdi.
- `.blend` ve `.fbx` dosyaları Git LFS takibine alındı.

### Alınan kararlar ve gerekçeleri
- Mevcut kişisel repo ve collaborator düzeniyle devam: organizasyon şu aşamada zorunlu değil.
- Büyük ikili sanat dosyaları için Git LFS kullan: model sürümlerini bu düzende takip et.
- Ajan işlemlerinde mümkün olduğunda doğrudan Git kullan; Desktop kullanıcının görsel kontrol aracı olarak kalsın.

### Önemli dosyalar
`.gitattributes`, repo başlangıç dosyaları.

### Kontroller
Git kurulum sonucu önceki kullanıcı ekranında görüldü. LFS kuralları bu belge oturumunda dosyadan doğrulandı. Noreply seçimi önceki kullanıcı bildirimidir; mevcut global hesap ayarı tekrar incelenmedi.

### Eksikler ve devam noktası
Ekipçe dal/inceleme politikası belirlenmedi. Kaynak model repoya eklenecek.

### Git kaydı
`83140cc` — Initial commit.
`f110d14` — Update .gitattributes.

---

## Oturum 2 — Leylek modeli, optimizasyon ve kaynak teslimi

### Amaç
Referanslardan yakın görülebilecek low poly ana leylek karakterini, iskeleti ve animasyonları hazırlamak; kaynağı korumak.

### Yapılanlar
- Önceki Blender çalışmasında model, 19 kemikli iskelet ve altı animasyon hazırlandı.
- Geometri 2602 üçgenden 1892 üçgene indirildi; vertex sayısı 1511'den 1116'ya düştü.
- Tek palet materyali kullanıldı; doku ve referanslar Blender dosyasına gömüldü.
- Idle'a hafif nefes ve baş/boyun hareketi; dalışa kısa bacak toplama, yeniden uzatma ve ayak eşlik hareketi eklendi.
- Kullanıcı son Blender kaynağını repoya kopyaladı, commit/push yaptı ve GitHub'da gördüğünü bildirdi.

### Alınan kararlar ve gerekçeleri
- Ana karakter yakından görüleceği için silueti koruyarak 1892 üçgende duruldu; daha düşük sayı zorlanmadı.
- Düzenlenebilir `.blend`, `ArtSource` altında kalacak. Unity çıktıları daha sonra Assets altında tutulacak.
- Animasyonlar: Idle, Walk, Takeoff, Fly, Glide ve Dive. Unity'de insan dışı karakter için Generic rig yaklaşımı planlandı; henüz uygulanmadı.

### Önemli dosyalar
`ArtSource/Stork/Stork_Hero_Lite.blend`.

### Kontroller
Geometri sayıları, 313 karelik koordinat/ağırlık kontrolleri ve dört döngünün uç poz eşleşmeleri **önceki Blender oturumunda** kaydedildi; bu belge çalışmasında Blender açılarak yeniden sınanmadı. Kaynak dosyasının repoda bulunduğu ve ilgili Git kaydı bu oturumda doğrulandı.

### Eksikler ve devam noktası
Bu optimize sürümün Unity aktarımı, materyali, Animator Controller'ı ve Prefab'ı doğrulanmadı. Yeni performans ölçümü yok.

### Git kaydı
`b162b2b` — Optimize leylek modelini ve animasyonlarını ekle.

---

## Oturum 3 — Unity projesi ve Git düzeni

### Amaç
Ekibin paylaşabileceği temiz bir low poly 3D Unity başlangıç projesi oluşturmak.

### Yapılanlar
- Kullanıcı Universal 3D şablonuyla UnityProject klasöründe proje oluşturdu.
- UnityProject altında Unity `.gitignore` dosyası eklendi; otomatik `.slnx` dosyası hariç tutuldu.
- Force Text ve Visible Meta Files ayarları kontrol edildi.
- Proje dosyaları Git kaydına alındı. Kullanıcıya commit/push akışı anlatıldı; uzak repo bu belge oturumunda ağdan kontrol edilmedi.

### Alınan kararlar ve gerekçeleri
- Unity 6000.3.27f1 ve URP ile başlangıç; ekip aynı tam Editor sürümünü kullanacak.
- Tek repo: sanat kaynakları `ArtSource/`, Unity projesi `UnityProject/`. İçeride ikinci repo oluşturulmayacak.
- Hub'da ek sürüm kontrol sağlayıcısı seçilmedi; Git/GitHub düzeni kullanılacak.
- `.meta`, Assets, Packages ve ProjectSettings takip edilecek; Library/Temp ve yerel çıktılar edilmeyecek.

### Önemli dosyalar
Kökteki `.gitignore`, `UnityProject/.gitignore`, `UnityProject/Assets`, `UnityProject/Packages`, `UnityProject/ProjectSettings`.

### Kontroller
- Unity sürümü, URP paketi, kayıt ayarları ve başlangıç sahnesi bu oturumdaki dosya incelemesiyle doğrulandı.
- Proje açılırken Windows Akıllı Uygulama Denetimi, `.NET Host` ve `Unity.Properties.SourceGenerator.dll` ile ilgili bildirimler gösterdi.
- Kullanıcı bildirimlerin durduğunu ve kısa Play/Console kontrolünün temiz olduğunu bildirdi. Bu, engellenen bileşenin tamamen düzeldiğinin kanıtı değildir.

### Eksikler ve devam noktası
Bildirim tekrarlanırsa ayrıntısı incelenecek; güvenlik koruması devre dışı bırakılmadı. `_Game` klasör düzeni ve Main sahnesi önerildi ancak henüz uygulanmış değil. Ekip arkadaşının bilgisayarında açılış testi yapılmadı.

### Git kaydı
`3546a68` — Unity URP projesini ve Git ayarlarını ekle.

---

## Oturum 4 — GitHub giriş sayfası ve ortak çalışma kaydı

### Amaç
README şablonunu hazırlamak; başka ajan ve ekip üyelerinin devam edebilmesi için işleri ve kararları tek günlükte toplamak.

### Yapılanlar
- Mevcut README, Git geçmişi, LFS kuralları, Unity sürümü/paketleri ve kayıt ayarları okundu.
- README; tanıtım, durum, kurulum, klasör yapısı ve görsel alanlarıyla yeniden düzenlendi.
- Bu tek günlükte geçmişin doğrulanabilen özeti ve yeni oturum şablonu oluşturuldu.
- AGENTS.md ile günlük güncelleme ve çalışma sınırları tanımlandı.
- Kullanıcının devam talebiyle README, ortalanmış başlık, renkli teknoloji rozetleri, bölüm bağlantıları, durum tablosu, açılır klasör ağacı ve işaretlenebilir yol haritasıyla düzenlendi. Kapak/görsel yer tutucuları kaldırıldı.

### Alınan kararlar ve gerekçeleri
- Kullanıcının tercihi: **tarih kullanılmayacak**, kayıtlar artan oturum numarasıyla sıralanacak.
- İş ve kararlar aynı günlükte bölüm bölüm tutulacak; ayrı karar/durum belgeleri açılmayacak.
- Güncel durum üstte, oturumlar aşağıda eski-yeni sırasıyla olacak.
- Önce şablon hazırlanacak; içerik ve gerçek görseller sonradan geliştirilecek.
- Sonraki kullanıcı tercihi önceki görsel planını güncelledi: kapak, karakter resmi veya GIF istenmiyor; klasik oyun reposu biçiminde renkli şablon kullanılacak. Renkler Shields.io rozetleri ve durum simgeleriyle verildi; özel CSS kullanılmadı. Rozetler dış hizmetten yüklenir, bilgi metin olarak da korunur.
- Bu çalışma yalnız yerel belge düzenlemesidir; commit ve push yetkisi kapsamda değil.

### Önemli dosyalar
`README.md`, `AGENTS.md`, `docs/CALISMA_GUNLUGU.md`.

### Kontroller
- Başlangıçta yerel Git değişiklik listesi boştu.
- Belge içi dosya bağlantıları, oturum sırası ve değişiklik kapsamı kontrol edildi.
- README gezinme bağlantıları için açık bölüm kimlikleri tanımlandı; görsel GitHub tarayıcı önizlemesi bu çalışmada yapılmadı.
- Unity veya Blender çalıştırılmadı; uzak repo sorgulanmadı.

### Eksikler ve devam noktası
Kullanıcı renkli şablonu inceleyecek. Oyun açıklaması ve açık kararlar daha sonra doldurulacak. Dış rozetlerin ağ üzerinden yüklenmesi bu çalışmada sınanmadı. Commit/push yapılmadı.

### Git kaydı
Henüz commit yok; üç belge yerel değişiklik olarak hazırlandı.

---

## Yeni oturum şablonu

Aşağıdaki boş şablonu kopyalayıp bu bölümün **üstüne**, son tamamlanan oturumun arkasına ekleyin. `N` yerine sıradaki numarayı yazın. Bilinmeyen alanı uydurmayın; `Henüz belirlenmedi` veya `Uygulanmadı` kullanın.

```markdown
## Oturum N — Kısa başlık

### Amaç

### Yapılanlar

### Alınan kararlar ve gerekçeleri

### Önemli dosyalar

### Kontroller
Gerçekten yapılan kontrol, sonucu ve varsa doğrulama sınırı.

### Eksikler ve devam noktası

### Git kaydı
Commit başlığı/kimliği veya Commit/push yapılmadı.

---
```
