# Storky — Çalışma günlüğü

İşler ve kararlar **tarih kullanılmadan, oturum sırasıyla** tek dosyada tutulur. Oturum bir kesintisiz çalışma bütünüdür; konu değişiklikleri aynı oturum içinde bölüm olarak yazılır. Kullanıcı düzeltmesiyle eski dört kayıt, içerikleri korunarak Oturum 1 altında dört bölüme toplandı. Bu geçmiş özeti önceki konuşma ve Git kayıtlarından yeniden oluşturulmuştur; geçmiş bölümlerdeki durum ifadeleri o çalışmanın kaydıdır. Güncel durum aşağıdadır.

## Güncel durum

| Konu | Durum ve dayanak |
| --- | --- |
| Repo | Storky-Game; yerel dal `main`; çalışma başlangıcında temizdi, mevcut değişiklikler korundu |
| Oturum | Bu çalışma **Oturum 2**; sohbet adı `Unity-Storky-2`. Bu çalışmanın devamı aynı kayıtta güncellenir |
| Hedef platform | Kullanıcı seçimi: önce Windows PC |
| Kaynak model | `ArtSource/Stork/Stork_Hero_Lite.blend` LFS ile takip ediliyor. Dışa aktarılan açık Blender kopyası eski çıktı klasöründeydi ve kaydedilmemiş değişiklikleri vardı; kaynak üzerine yazılmadı |
| Unity | 6000.3.27f1, URP 17.3.0, Input System 1.20.1; Editor yükseltilmedi |
| Bağlantı | Unity CLI 1.0.0-beta.15 + Pipeline 0.9.1-exp.1; Codex MCP üzerinden canlı okuma/yazma ve Play kontrolleri doğrulandı |
| Sahne | `UnityProject/Assets/_Game/Scenes/FlightTest.unity`; 300×300 m çizgili eğitim alanı. Eski SampleScene korundu |
| Karakter | `_Game/Art/Stork` altında FBX ve PNG; Generic rig, tek palet materyali, altı klip, Animator ve Prefab hazır |
| Prototip | Yeni Input System ile yürüyüş, kalkış, uçuş, süzülme, dalış, otomatik iniş, takip kamerası ve cihaz ailesine göre kontrol metinleri |
| Kontroller | Sanal klavye/gamepad ile temel davranışlar sınandı; fiziksel XInput cihazı algılandı; gerçek kullanıcı testi bekleniyor |
| Git | Commit/push yapılmadı. Unity değişiklikleri ve bu belge düzenlemeleri yereldedir; GitHub'a gönderilmedi |

### Sıradaki işler

1. Kullanıcı FlightTest sahnesini klavye ve bağlı XInput kontrolcüsüyle deneyecek; hız, dönüş, kamera ve animasyon geçişleri geri bildirime göre ayarlanacak.
2. Açık Blender kopyası ile repodaki kaynak arasındaki farklar incelenecek; kaydedilmemiş değişiklikler otomatik olarak repoya taşınmayacak.
3. PlayStation/Switch fiziksel kontrolcü testi ve ekip arkadaşının bilgisayarında proje açılışı yapılacak.
4. Görsel bütçe önerileri kesinleştirilecek; hedef PC donanımı ve temsili sahneyle performans ölçülecek.
5. Kullanıcının göndereceği tekrar kullanılabilir başlangıç metni AGENTS.md'ye eklenecek; henüz gönderilmedi.
6. GitHub'a gönderilecek kapsam netleştirilip açık commit/push talimatına göre işlem yapılacak.

### Açık kararlar

- Oyuncunun temel amacı, oyun döngüsü ve nihai uçuş modeli.
- Hedef PC donanımı, performans ölçümü ve görsel bütçelerin kesin üst sınırları.
- Ekip görevleri, dal/inceleme düzeni ve dağıtım/lisans koşulları.
- Mevcut README tercihi: görselsiz renkli şablon; kapak, karakter resmi veya GIF eklenmez.

---

## Oturum 1 — Proje temeli, karakter kaynağı ve ortak belgeler

Önceden ayrı oturum olarak numaralanan aşağıdaki dört konu, kullanıcının bildirimine göre aynı oturumda yapıldı. Yalnızca başlık hiyerarşisi değiştirildi; bölüm içerikleri ve tarihsel Git kayıtları korunmuştur.

### Bölüm 1 — Repo ve büyük dosya takibi

#### Amaç
Ortak GitHub reposunu ve sanat dosyaları için sürüm kontrolünü hazırlamak.

#### Yapılanlar
- Kullanıcı Git ve GitHub Desktop kurulumunu tamamladı; Storky-Game reposunu yerel bilgisayara aldı.
- Kullanıcı commit kimliği için GitHub noreply adresini seçtiğini bildirdi.
- `.blend` ve `.fbx` dosyaları Git LFS takibine alındı.

#### Alınan kararlar ve gerekçeleri
- Mevcut kişisel repo ve collaborator düzeniyle devam: organizasyon şu aşamada zorunlu değil.
- Büyük ikili sanat dosyaları için Git LFS kullan: model sürümlerini bu düzende takip et.
- Ajan işlemlerinde mümkün olduğunda doğrudan Git kullan; Desktop kullanıcının görsel kontrol aracı olarak kalsın.

#### Önemli dosyalar
`.gitattributes`, repo başlangıç dosyaları.

#### Kontroller
Git kurulum sonucu önceki kullanıcı ekranında görüldü. LFS kuralları bu belge oturumunda dosyadan doğrulandı. Noreply seçimi önceki kullanıcı bildirimidir; mevcut global hesap ayarı tekrar incelenmedi.

#### Eksikler ve devam noktası
Ekipçe dal/inceleme politikası belirlenmedi. Kaynak model repoya eklenecek.

#### Git kaydı
`83140cc` — Initial commit.
`f110d14` — Update .gitattributes.

---

### Bölüm 2 — Leylek modeli, optimizasyon ve kaynak teslimi

#### Amaç
Referanslardan yakın görülebilecek low poly ana leylek karakterini, iskeleti ve animasyonları hazırlamak; kaynağı korumak.

#### Yapılanlar
- Önceki Blender çalışmasında model, 19 kemikli iskelet ve altı animasyon hazırlandı.
- Geometri 2602 üçgenden 1892 üçgene indirildi; vertex sayısı 1511'den 1116'ya düştü.
- Tek palet materyali kullanıldı; doku ve referanslar Blender dosyasına gömüldü.
- Idle'a hafif nefes ve baş/boyun hareketi; dalışa kısa bacak toplama, yeniden uzatma ve ayak eşlik hareketi eklendi.
- Kullanıcı son Blender kaynağını repoya kopyaladı, commit/push yaptı ve GitHub'da gördüğünü bildirdi.

#### Alınan kararlar ve gerekçeleri
- Ana karakter yakından görüleceği için silueti koruyarak 1892 üçgende duruldu; daha düşük sayı zorlanmadı.
- Düzenlenebilir `.blend`, `ArtSource` altında kalacak. Unity çıktıları daha sonra Assets altında tutulacak.
- Animasyonlar: Idle, Walk, Takeoff, Fly, Glide ve Dive. Unity'de insan dışı karakter için Generic rig yaklaşımı planlandı; henüz uygulanmadı.

#### Önemli dosyalar
`ArtSource/Stork/Stork_Hero_Lite.blend`.

#### Kontroller
Geometri sayıları, 313 karelik koordinat/ağırlık kontrolleri ve dört döngünün uç poz eşleşmeleri **önceki Blender oturumunda** kaydedildi; bu belge çalışmasında Blender açılarak yeniden sınanmadı. Kaynak dosyasının repoda bulunduğu ve ilgili Git kaydı bu oturumda doğrulandı.

#### Eksikler ve devam noktası
Bu optimize sürümün Unity aktarımı, materyali, Animator Controller'ı ve Prefab'ı doğrulanmadı. Yeni performans ölçümü yok.

#### Git kaydı
`b162b2b` — Optimize leylek modelini ve animasyonlarını ekle.

---

### Bölüm 3 — Unity projesi ve Git düzeni

#### Amaç
Ekibin paylaşabileceği temiz bir low poly 3D Unity başlangıç projesi oluşturmak.

#### Yapılanlar
- Kullanıcı Universal 3D şablonuyla UnityProject klasöründe proje oluşturdu.
- UnityProject altında Unity `.gitignore` dosyası eklendi; otomatik `.slnx` dosyası hariç tutuldu.
- Force Text ve Visible Meta Files ayarları kontrol edildi.
- Proje dosyaları Git kaydına alındı. Kullanıcıya commit/push akışı anlatıldı; uzak repo bu belge oturumunda ağdan kontrol edilmedi.

#### Alınan kararlar ve gerekçeleri
- Unity 6000.3.27f1 ve URP ile başlangıç; ekip aynı tam Editor sürümünü kullanacak.
- Tek repo: sanat kaynakları `ArtSource/`, Unity projesi `UnityProject/`. İçeride ikinci repo oluşturulmayacak.
- Hub'da ek sürüm kontrol sağlayıcısı seçilmedi; Git/GitHub düzeni kullanılacak.
- `.meta`, Assets, Packages ve ProjectSettings takip edilecek; Library/Temp ve yerel çıktılar edilmeyecek.

#### Önemli dosyalar
Kökteki `.gitignore`, `UnityProject/.gitignore`, `UnityProject/Assets`, `UnityProject/Packages`, `UnityProject/ProjectSettings`.

#### Kontroller
- Unity sürümü, URP paketi, kayıt ayarları ve başlangıç sahnesi bu oturumdaki dosya incelemesiyle doğrulandı.
- Proje açılırken Windows Akıllı Uygulama Denetimi, `.NET Host` ve `Unity.Properties.SourceGenerator.dll` ile ilgili bildirimler gösterdi.
- Kullanıcı bildirimlerin durduğunu ve kısa Play/Console kontrolünün temiz olduğunu bildirdi. Bu, engellenen bileşenin tamamen düzeldiğinin kanıtı değildir.

#### Eksikler ve devam noktası
Bildirim tekrarlanırsa ayrıntısı incelenecek; güvenlik koruması devre dışı bırakılmadı. `_Game` klasör düzeni ve Main sahnesi önerildi ancak henüz uygulanmış değil. Ekip arkadaşının bilgisayarında açılış testi yapılmadı.

#### Git kaydı
`3546a68` — Unity URP projesini ve Git ayarlarını ekle.

---

### Bölüm 4 — GitHub giriş sayfası ve ortak çalışma kaydı

#### Amaç
README şablonunu hazırlamak; başka ajan ve ekip üyelerinin devam edebilmesi için işleri ve kararları tek günlükte toplamak.

#### Yapılanlar
- Mevcut README, Git geçmişi, LFS kuralları, Unity sürümü/paketleri ve kayıt ayarları okundu.
- README; tanıtım, durum, kurulum, klasör yapısı ve görsel alanlarıyla yeniden düzenlendi.
- Bu tek günlükte geçmişin doğrulanabilen özeti ve yeni oturum şablonu oluşturuldu.
- AGENTS.md ile günlük güncelleme ve çalışma sınırları tanımlandı.
- Kullanıcının devam talebiyle README, ortalanmış başlık, renkli teknoloji rozetleri, bölüm bağlantıları, durum tablosu, açılır klasör ağacı ve işaretlenebilir yol haritasıyla düzenlendi. Kapak/görsel yer tutucuları kaldırıldı.

#### Alınan kararlar ve gerekçeleri
- Kullanıcının tercihi: **tarih kullanılmayacak**, kayıtlar artan oturum numarasıyla sıralanacak.
- İş ve kararlar aynı günlükte bölüm bölüm tutulacak; ayrı karar/durum belgeleri açılmayacak.
- Güncel durum üstte, oturumlar aşağıda eski-yeni sırasıyla olacak.
- Önce şablon hazırlanacak; içerik ve gerçek görseller sonradan geliştirilecek.
- Sonraki kullanıcı tercihi önceki görsel planını güncelledi: kapak, karakter resmi veya GIF istenmiyor; klasik oyun reposu biçiminde renkli şablon kullanılacak. Renkler Shields.io rozetleri ve durum simgeleriyle verildi; özel CSS kullanılmadı. Rozetler dış hizmetten yüklenir, bilgi metin olarak da korunur.
- Bu çalışma yalnız yerel belge düzenlemesidir; commit ve push yetkisi kapsamda değil.

#### Önemli dosyalar
`README.md`, `AGENTS.md`, `docs/CALISMA_GUNLUGU.md`.

#### Kontroller
- Başlangıçta yerel Git değişiklik listesi boştu.
- Belge içi dosya bağlantıları, oturum sırası ve değişiklik kapsamı kontrol edildi.
- README gezinme bağlantıları için açık bölüm kimlikleri tanımlandı; görsel GitHub tarayıcı önizlemesi bu çalışmada yapılmadı.
- Unity veya Blender çalıştırılmadı; uzak repo sorgulanmadı.

#### Eksikler ve devam noktası
Kullanıcı renkli şablonu inceleyecek. Oyun açıklaması ve açık kararlar daha sonra doldurulacak. Dış rozetlerin ağ üzerinden yüklenmesi bu çalışmada sınanmadı. Commit/push yapılmadı.

#### Git kaydı
Henüz commit yok; üç belge yerel değişiklik olarak hazırlandı.

---

## Oturum 2 — Unity bağlantısı, leylek aktarımı ve uçuş prototipi

### Amaç
Unity ile doğrudan çalışabilmek, mevcut animasyonlu leyleği kaynaklarını koruyarak aktarmak ve Windows PC için kullanıcı tarafından denenebilir bir yürüyüş/uçuş alanı hazırlamak.

### Bölüm 1 — Başlangıç ve bağlantı
- AGENTS.md, README.md ve bu günlük okundu; dal `main`, başlangıç Git çalışma ağacı temizdi.
- Unity sürümü ve paketleri dosyalardan doğrulandı. Başlangıç repo taramasında FBX bulunmadı.
- Resmî CLI/Pipeline yolu, CoplayDev ve IvanMurzak MCP seçenekleri araştırıldı. Resmî bağlantıyla ilerleme kararı alındı.
- Kullanıcı CLI beta.13 kurulumunu, hesap girişini, Pipeline kurulumunu ve Codex yapılandırmasını komut çıktılarıyla paylaştı. İlk erişilememe durumu Unity yeniden açıldıktan sonra düzeldi.
- Beta.15 sürüm notları incelendi; Pipeline 0.9 MCP araç adı çakışması düzeltmesi nedeniyle güncelleme önerildi. Kullanıcı güncelledi; sürüm ve Editor bağlantısı çıktılardan doğrulandı.
- `unity mcp configure codex`, kullanıcı düzeyindeki Codex ayarını oluşturdu ve yerel ağ erişim kısıtlamasını gevşettiğini bildirdi. Bu ayar reponun parçası değildir.
- Codex yeniden açıldıktan sonra MCP ile doğru proje yolu, Editor sürümü, SampleScene hiyerarşisi ve temiz Console doğrudan okundu.
- Kurulu paketten otomatik sunucu başlangıcı, Project Settings > Pipeline > Editor ve Window > Pipeline > Start Server menüleri doğrulandı. PowerShell'in sürekli açık kalması gerekmiyor.

### Bölüm 2 — Blender incelemesi ve çıktı
- Canlı Blender 5.2.3 LTS üzerinden 1.892 üçgen, 1.116 kaynak vertex, 19 kemik, tek materyal, 128×128 gömülü palet ve vertex başına en çok iki kemik etkisi doğrulandı.
- Altı ayrı Action'ın NLA şeridine dizildiği görüldü: Idle 1–61, Walk 62–102, Takeoff 103–175, Fly 176–206, Glide 207–267, Dive 268–313; kaynak 30 FPS.
- Açık kaynak, önceki `Leylek_Hero_Lite` çıktı klasöründeydi; kaydedilmemiş değişiklikleri vardı. Repodaki `.blend` ile eşitliği doğrulanmadı ve iki kaynağın da üzerine yazılmadı.
- Seçili mesh ve rig FBX olarak dışa aktarıldı: NLA Strips açık, All Actions kapalı, ek leaf bone kapalı, örnekleme 1, sadeleştirme 0, -Z forward / Y up. Stüdyo ve referans nesneleri aktarılmadı.
- FBX ve PNG bu sohbetin `outputs/Stork_Unity` klasörüne alındı; FBX dosyasında altı ayrı animasyon yığını, PNG'de 128×128 boyut doğrulandı. Bunlar ayrıca Unity Assets altına aktarıldı.
- Unity mesh'i 1.892 üçgen ve 4.633 vertex olarak raporladı; Unity vertex sayısı Blender kaynak vertex sayısından farklıdır.

### Bölüm 3 — Unity prototipi
- `_Game` altında sanat, script, editor kurulumu, materyal, doku, Animator, Prefab ve sahne klasörleri oluşturuldu.
- FBX Generic rig ve Root düğümüyle kuruldu. Idle/Walk/Fly/Glide döngülü; Takeoff/Dive tek seferlik. Root motion kapalı; hareket CharacterController tarafından yürütülüyor.
- URP palet materyali ve noktasal filtrelenen palet dokusu kuruldu; Animator'da altı durum oluşturuldu.
- FlightTest sahnesine 300×300 m düz zemin, yaklaşık 1 m kareli/10 m kalın çizgili tekrarlı doku, yön şeritleri ve 25 m aralıklı mesafe işaretleri eklendi.
- Yeni Input System InputActionMap ile klavye ve gamepad hareketi, kalkış, yükseklik kontrolü, süzülme, dalış, sıfırlama ve kamera kuruldu. Uçuş otomatik ileri hareket ediyor; alçalınca zemine temasla yürüyüşe dönüyor. Alan ve yükseklik sınırı var.
- Ekrandaki kontrol rehberi son kullanılan cihaz ailesine göre klavye, Xbox, PlayStation, Switch veya genel gamepad metinlerini gösteriyor. XInput olarak tanıtılan cihazın gerçek markası kesin olarak ayırt edilemez; Xbox gösterimi kullanılır.
- Başlangıç kontrolleri: WASD/sol çubuk hareket; sağ fare basılı+fare/sağ çubuk kamera; Space/güney tuşu kalkış; Space/RT yükselme; Ctrl/LT alçalma; Shift/batı tuşu süzülme; E/doğu tuşu dalış; R/Select başlangıca dönüş.
- Kullanıcı telefona bitiş bildirimi istedi. Doğrudan telefon bildirimi aracı bulunmadığından bildirim gönderilmedi; sohbette hazır olduğu bildirildi.

### Kararlar / Decisions
- İlk hedef platform **Windows PC**; kullanıcı tarafından seçildi.
- Düzenlenebilir kaynak `.blend` ArtSource altında korunacak; Unity'de FBX + ayrı PNG, Generic rig ve altı adlandırılmış klip kullanılacak. Kaynak ile çıktı eşleştirmesi açık iş olarak duruyor.
- Yeni Input System ve CharacterController ilk kontrol prototipinin temeli. Hızlar, kalkış süresi, kamera ve tuşlar kullanıcı testinden sonra değişebilir.
- Low poly için evrensel tek üçgen sınırı kabul edilmedi. Mevcut leyleğin silueti ve sade paleti görsel referans önerisi olarak alındı; daha fazla azaltma şu anda yapılmadı.
- **Kesinleşmemiş başlangıç bütçesi önerileri:** ana karakter 1.500–3.000 üçgen, diğer hayvanlar 500–2.000, küçük nesneler 50–500, ağaç/büyük çevre nesneleri 300–2.000. Büyük yapılar parça/görünürlük üzerinden değerlendirilecek. Bunlar ölçülmüş performans garantisi veya onaylanmış katı sınırlar değildir.
- Önerilen teknik hedefler: karakterde tek materyal/tek skinned mesh, en çok 30 kemik ve vertex başına en çok 4 etki; palet 128–256 px, özel doku 512 px, yakın plan gerekirse 1024 px; 1 Unity birimi = 1 m. Kaynak animasyon 30 FPS; 1080p/60 FPS oyun hedefi donanım ve sahne ölçümüyle doğrulanacak.
- Kullanıcı oturum düzenini düzeltti: önceki dört konu Oturum 1'in bölümleri; bu çalışma Oturum 2. Sohbet adı `Unity-Storky-2`; yeniden açma ve konu değişimi yeni oturum oluşturmaz.
- Günlük/karar yazımı normalde çalışma sonunda kullanıcı onayıyla yapılır. Bu düzenleme için kullanıcı bu mesajında açık yetki verdi. Commit/push için ayrıca açık yetki aranır.

### Önemli dosyalar
- `UnityProject/Assets/_Game/Scenes/FlightTest.unity`
- `UnityProject/Assets/_Game/Scripts/StorkPrototype.cs`
- `UnityProject/Assets/_Game/Editor/StorkPrototypeSetup.cs`
- `UnityProject/Assets/_Game/Art/Stork/Stork_Hero.fbx` ve `Stork_Palette.png`
- `UnityProject/Assets/_Game/Animations/Stork.controller`
- `UnityProject/Assets/_Game/Prefabs/StorkPlayer.prefab`
- `_Game/Materials`, `_Game/Textures`, paket manifesti/kilidi ve Unity'nin ürettiği proje ayarları; `.meta` dosyaları korundu.
- `AGENTS.md`, `README.md`, `docs/CALISMA_GUNLUGU.md` oturum düzeni ve güncel durum için düzenlendi.

### Kontroller
- İlk iki derleme hatası kurulu Input System API'sine uyum sağlanarak giderildi. Son derleme başarılı; Console'un güncel hata/uyarı sayısı sıfırdı.
- Sanal klavyeyle Walk, Fly, Glide ve iniş sonrası Idle durumları; sanal gamepad ile Walk, Fly, Dive ve Glide durumları ve konum değişimleri gözlendi.
- Fiziksel Xbox/XInput kontrolcüsü Unity tarafından algılandı. PlayStation ve Switch aile metinleri sanal cihazlarla sınandı; gerçek donanım testi yapılmadı.
- Game görüntülerinde zemin, palet, leylek, uçuş animasyonu ve HUD incelendi. Geçici sanal cihazlar kaldırıldı; son cihaz listesinde gerçek klavye, fare ve XInput kaldı.
- Son durumda Play kapalı, FlightTest açık ve kaydedilmişti; geçici arka planda çalıştırma override'ı kapatıldı. Kullanıcının mevcut SampleScene'i korunuyor.
- Git diff biçim kontrolü geçti; commit/push yapılmadı. Unity testi bu belge düzenlemesi için gereksiz yere tekrarlanmadı.
- Oturum 1 içeriği başlıklar dışında değişmeden korundu; oturum numaraları ve belge tutarlılığı kontrol edildi.

### Eksikler ve devam noktası
Gerçek kullanıcı oyun testi, fiziksel PlayStation/Switch testi, hedef PC performans ölçümü ve başka bilgisayarda açılış testi bekleniyor. Kaynak `.blend` farkları incelenecek. Tuş göstergeleri metin tabanlıdır; özel kontrolcü ikonları yoktur. Nihai oyun döngüsü, iniş animasyonu ve son uçuş fiziği bu prototipte tamamlanmış sayılmaz. Kullanıcının göndereceği başlangıç metni ve GitHub'a gönderim kapsamı bekleniyor.

### Git kaydı
Commit/push yapılmadı. Oturum başında temiz olan çalışma ağacında bu çalışmanın Unity ve belge değişiklikleri bulunuyor. GitHub'daki belgeler henüz güncellenmedi.

---

## Yeni oturum şablonu

Aşağıdaki boş şablonu kopyalayıp bu bölümün **üstüne**, son tamamlanan oturumun arkasına ekleyin. `N` yerine sıradaki numarayı yazın. Bilinmeyen alanı uydurmayın; `Henüz belirlenmedi` veya `Uygulanmadı` kullanın.

```markdown
## Oturum N — Kısa başlık

### Amaç

### Yapılanlar

### Kararlar / Decisions

### Önemli dosyalar

### Kontroller
Gerçekten yapılan kontrol, sonucu ve varsa doğrulama sınırı.

### Eksikler ve devam noktası

### Git kaydı
Commit başlığı/kimliği veya Commit/push yapılmadı.

---
```
