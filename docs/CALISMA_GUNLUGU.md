# Storky — Çalışma günlüğü

İşler ve kararlar **tarih kullanılmadan, oturum sırasıyla** tek dosyada tutulur. Her oturum kesintisiz bir çalışmayı, bölümler o çalışmanın konularını temsil eder. Üstteki Güncel durum son yetkili güncellemeyi gösterir; aşağıdaki eski kayıtlar kendi dönemlerinin tarihçesidir. Genel yönergede geçerli oturum numarası sabitlenmez. Kullanıcının açık oturum düzeltmesi kayıt güncellenmeden önce de geçerlidir.

## Güncel durum

| Konu | Durum ve dayanak |
| --- | --- |
| Repo | Storky-Game; yerel dal `main`; ada ve hareket değişiklikleri yerelde, mevcut çalışma ağacı korundu. Uzak depo bu çalışmada sorgulanmadı |
| Oturum | **Oturum 3**; kullanıcı tarafından açıkça belirlendi. İlgili sohbet `Unity-Storky-3` olarak yeniden adlandırıldı; aynı çalışmanın devamı bu kayıtta tutulur |
| Hedef platform | Kullanıcı seçimi: önce Windows PC |
| Oyun tasarımı | Kullanıcının sağladığı [Storky_GDD.pdf](Storky_GDD.pdf) repoya eklendi. Terfi yarışı, iki ana ada ve keşif/görev kapsamı tasarım olarak tanımlı; görev, diyalog ve kayıt sistemleri henüz uygulanmadı |
| Sanat kaynakları | Leylek: `ArtSource/Stork/Stork_Hero_Lite.blend`; ada: `ArtSource/Environment/BirdIsland/Storky_BirdIsland.blend`. Ada FBX'i Unity'ye aktarıldı; eski leylek kaynak-kopya farkı açık iş olarak korunuyor |
| Unity | 6000.3.27f1, URP 17.3.0, Input System 1.20.1; Editor/paket sürümleri değiştirilmedi |
| Bağlantı | Bu oturumdaki ürün çalışmasında Blender MCP ve Unity Pipeline kullanıldı; CLI 1.0.0-beta.16'ya güncellendi. Bağlantı her yeni çalışmada yeniden doğrulanmalı |
| Sahne | `UnityProject/Assets/_Game/Scenes/FlightTest.unity`: 600×600 m Kuş Adası, 70 dış bina kütlesi, fabrika avlusu başlangıcı. Eski test nesneleri kaldırıldı; SampleScene ve proje varlıkları korunuyor |
| Deniz / görünüm | İki üçgenli, kamerayı takip eden dokusuz mavi deniz; tek güneş; sis ve uzak görünüm/gölge ayarları. Ada tamamen gri, iç mekân yok |
| Karakter / hareket | Mevcut rig ve altı Animator durumu korundu; ayrı kalibre yürüyüş klibi, gerçek hıza göre tempo, gövde merkezli Air Roll, stamina, momentum ve dalıştan toparlanma desteği hazır |
| Kontroller | Serbest fare kamerası; W/S burun aşağı/yukarı, Q/E roll, Space tek vuruş, Shift dalış. Gamepad karşılıkları ve ayrıntılar aşağıda ve README'de |
| Doğrulama | Ada kurulumunda 18 davranış kontrolü; hareket düzeltmesinde 13 hedefli kontrol geçti. Son ürün kontrolünde Console hatasız/uyarısız, Play kapalı ve sahne kaydedilmişti. Bunlar donanım performansı veya tüm oyun için tam test kanıtı değildir |
| Git | Commit/push yapılmadı. Son görülen mevcut commit `05492b8` (`unity baslangici ve diger degisiklikler`); bu oturumun ürün ve belge değişiklikleri henüz gönderilmedi |

### Sıradaki işler

1. Kullanıcı, FlightTest'te yürüyüş temposu, gövde merkezli Air Roll, dalıştan çıkış ve kanat çırpma etkisini deneyecek; denge buna göre ayarlanacak.
2. Yamaçlarda/çatılarda ayak teması, konma ve kamera çarpışmaları daha geniş güzergâhlarda kontrol edilecek. Düz zemin testi bütün araziyi doğrulamaz.
3. Fiziksel gamepad, özellikle PlayStation/Switch, başka ekip bilgisayarında açılış ve hedef Windows donanımında performans ölçümü yapılacak.
4. GDD'nin görev/diyalog/kayıt sistemleri ile İnsan Adası ve küçük keşif adaları kullanıcı önceliğine göre planlanacak; tamamlanmış sayılmayacak.
5. Eski açık leylek Blender kopyası ile repodaki kaynağın farkı incelenecek; bu açık iş [Oturum 2](#oturum-2--unity-bağlantısı-leylek-aktarımı-ve-uçuş-prototipi)'den devrediyor.
6. Tekrar kullanılabilir başlangıç metni kullanıcı tarafından verilirse tek kopya olarak yönergelere eklenecek. Commit/push kapsamı ancak açık kullanıcı talimatıyla uygulanacak.

### Açık kararlar

- Nihai yürüyüş/uçuş dengesi ve dalıştan toparlanma desteğinin son değerleri. Mevcut sayılar test ayarlarıdır.
- Hedef PC donanımı, performans bütçesi ve ölçüm sonucu; sabit FPS garantisi verilmedi.
- Görevlerin uygulanma sırası, ikinci ada ve keşif adalarının ayrıntılı yerleşimi.
- Ekip görevleri, dal/inceleme düzeni ve dağıtım/lisans koşulları.
- README tercihi korunuyor: renkli klasik şablon; kapak, karakter resmi veya GIF eklenmez.

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

## Oturum 3 — Kuş Adası greybox'ı, Unity sahnesi ve hareket düzeni

### Amaç

Kullanıcının GDD'sine ve seçtiği ölçeğe göre Kuş Adası'nın dış mekân greybox'ını kurmak; mevcut leyleği koruyarak Unity'ye aktarmak; deniz, ışık, görünürlük ve hareket sistemini kullanıcı kontrolleriyle hazırlamak. Sonraki geri bildirimle yürüyüş, Air Roll ve dalıştan toparlanmayı düzeltmek.

### Bölüm 1 — Tasarım ve Blender

- Kullanıcı önce denizin Unity'de kurulması yaklaşımını kabul etti; modellemeye başlamadan ada türü, büyüklük ve görünüm soruldu.
- Kullanıcı **Kuş Adası**, fabrika, **600×600 m / 60–90 bina**, Ege/Akdeniz yamaçları, koy ve liman, tamamen gri ve yalnız dış mekân seçti. İç mekân yapılmadı.
- Blender MCP ile 70 bina kütlesi, mahalleler, fabrika yerleşkesi, meydan, çarşı, kamusal yapılar, liman, çiftlikler, deniz feneri, konma yüzeyleri ve yollar oluşturuldu.
- Son çevre sayımı: ağaç kopyaları dahil 14.210 üçgen, 152 görünür mesh nesnesi, dört ortak nötr gri materyal ve sıfır doku görseli. Ana ada ölçüsü 600×600 m olarak doğrulandı.
- Bina sınır kutuları ve yol merkezleri kontrol edildi; bulunan yol-bina çakışmaları düzeltildi. Rıhtım/iskele rampaları eklendi. Bunlar tam fizik veya bütün rota testi değildir.
- Düzenlenebilir ada `.blend` kaynağı `ArtSource/Environment/BirdIsland/`, Unity için üçgenlenmiş metre ölçekli FBX `_Game/Art/Environment/BirdIsland/` altına kaydedildi. Kamera, ışık, animasyon ve gizli ağaç prototipi FBX'e dahil edilmedi.

### Bölüm 2 — Unity, deniz ve onaylanan kontroller

- Kullanıcı silme kapsamını açık sahnedeki kuş dışındaki nesneler olarak onayladı. Mevcut Stork Player, rig, palet ve animasyonlar korundu; eski eğitim zemini/işaretleri kaldırıldı. Diğer proje varlıkları silinmedi.
- Ada collider'ları ve tekrar kullanılabilir BirdIsland Prefab'ı kuruldu; başlangıç fabrika avlusu, denize düşünce başlangıca dönüş olarak ayarlandı.
- İlk Play kontrolünde FBX ölçeğiyle büyüyen ağaç kapsülleri saptandı ve geometriyle eşleşen mesh collider'larla düzeltildi.
- Tek yönlü güneş, gri çevre materyalleri, dokusuz gökyüzü ve kamerayı takip eden iki üçgenli mavi deniz eklendi. Gerçek zamanlı yansıma, kırılma veya saydam su katmanları kullanılmadı.
- Görüş 3.000 m, küçük detay uzaklığı 330 m, evler için 1.500 m üst sınırı ve ekran boyutuna göre gizleme kuruldu. Arazi silueti korunuyor. Gölgeler 110 m, iki kademe ve 2048 çözünürlük; 2x MSAA ve SRP Batcher açık, çevre statik birleştirme için işaretli. Gereksiz kamera renk/derinlik kopyaları kapalı.
- Deniz sisinin çok büyük dört köşeden hesaplanması yakın suyu da soldurduğu için sis hesabı yüzey boyunca yapıldı; ufuk ve gökyüzü renkleri eşleştirildi. URP'nin desteklemediği spherical layer culling seçeneği kaldırıldı. Uzaklık değerleri oyun açılışında da uygulanıyor.
- Kullanıcı fare kamerasını sağ tuş tutmadan istedi. Klavye: yerde WASD; havada W/S burun aşağı/yukarı, A/D dönüş, Q/E Air Roll, Space her basışta kalkış/kanat çırpma, Shift basılı dalış, R başlangıca dönüş, Esc duraklatma/devam. F etkileşim için ayrıldı; görev/diyalog uygulanmadı.
- Gamepad: sol çubuk yürüme/burun/dönüş, sağ çubuk kamera, LB/RB veya L1/R1 roll, A/Çarpı tek vuruş, B/Daire dalış, X/Kare ayrılmış etkileşim girişi, View/Share dönüş, Start/Options duraklatma.
- Önceki test alanının sabit yatay/yükseklik sınırları ve doğrudan yükselme/alçalma kontrolü kaldırıldı; momentum, sınırlı kanat çırpma, otomatik süzülme ve hafif hava akımı eklendi. Hava akımı ilave stamina doldurmuyor.

### Bölüm 3 — Yürüyüş, gövde merkezli roll ve dalıştan toparlanma

- Kullanıcı yürüyüşte kayma, merkezden uzak Air Roll, dalıştan çıkışın zayıflığı ve kanat vuruşunun ileri hız katkısıyla ilgili geri bildirim verdi. Bu değişiklikleri doğrudan talep etti.
- Orijinal yürüyüşün yerdeki ayak hareketi yaklaşık 0,35 m/s'ye uygunken karakter 4 m/s ilerliyordu. Özgün FBX değiştirilmeden daha uzun adımlı, bacakları çözümlenmiş ayrı bir yürüyüş klibi oluşturuldu; Animator'daki Walk durumuna bağlandı.
- Ölçülen yeni klip referansı yaklaşık 0,805 m/s. Uygulanan yürüyüş hızı 1,4 m/s; animasyon temposu istenen girişten değil gerçek kat edilen mesafeden hesaplanıyor. Analog girişte tempo azalıyor; duvarda hareket durunca Idle'a dönüyor.
- Air Roll, Animator pozu değerlendirildikten sonra gövde kemiği etrafında uygulanıyor. Roll'un eklediği otomatik yaw ve dairesel rota etkisi kaldırıldı; açısal hızla dönüş ve bırakınca açının korunması sağlandı.
- Gerçek dalışta kaybedilen irtifadan kısa süreli toparlanma desteği birikiyor. Dalış bırakılıp burun kaldırılırken bu destek stamina olmadan kullanılabiliyor; yalnız tuşa basarak dolmuyor ve kullanımda tüketiliyor. Yükselişte momentum daha güçlü korunuyor.
- Uygulanan deneme değerleri: en yüksek hava hızı 62 m/s, roll hızı 220 derece/s, havadaki kanat vuruşu ileri hız katkısı 5,5 m/s. Toparlanma katkısı biriken desteğe bağlı; azami hız katkısı 11 m/s ve süresi 2 s. Bunlar kullanıcının geri bildirimine göre değiştirilebilen prototip değerleridir.
- Sahne ve karakter Prefab'ındaki ayarlar eşleştirildi; ada kurulum kodundaki eski hız değerleri de güncellendi.

### Bölüm 4 — Kayıt ve oturum düzeni

- Kullanıcı bu çalışmanın **Oturum 3** olduğunu açıkça belirtti; ilgili sohbet adı araç sonucuyla `Unity-Storky-3` olarak değiştirildi.
- Eski AGENTS.md geçerli çalışmayı ve sohbet adını Oturum 2'ye sabitliyordu. Kullanıcının onayıyla bu ifadeler genel kuralla değiştirildi: numara tek yerde günlüğün Güncel durum bölümünde tutulur; kullanıcının açık düzeltmesi önceliklidir; geçmiş oturum metni yeni çalışmayı belirlemez.
- Kullanıcı bu oturum kaydının, README'nin ve yönergelerin güncellenmesini açıkça onayladı. Bu onay commit/push yetkisi olarak yorumlanmadı.
- Sağlanan GDD, kaynak PDF değiştirilmeden `docs/Storky_GDD.pdf` olarak repoya eklendi. README güncel oyun tanımı, ada sahnesi, kaynak yolları, kontroller, yol haritası ve doğrulama sınırlarıyla eşleştirildi. Geçmiş iki oturumun metni korundu.

### Kararlar / Decisions

- **Kullanıcı tercihleri:** Kuş Adası; 600×600 m, 60–90 bina ölçeği; gri/dokusuz dış mekân; fabrika ve ada kasabası; deniz Unity'de ve mavi; mevcut leyleği koruyarak yalnız sahne nesnelerini yenileme; serbest fare kamerası ve yukarıdaki kontroller.
- **Hareket yönü:** Gövde merkezli, Rocket League benzeri Air Roll hissi; stamina bitse de dalıştan kazanılan momentumla toparlanma; kanat vuruşunda daha güçlü ileri hız katkısı. Bunlar kullanıcının bu oturumdaki yeni talepleridir. [Oturum 2](#oturum-2--unity-bağlantısı-leylek-aktarımı-ve-uçuş-prototipi)'deki ilk test kontrolleri ve sabit alan yaklaşımı güncel tercih değildir; tarihçe olarak korunur.
- **Uygulanan prototip ayarları:** Yürüyüş, hızlar, destek süresi, sis ve görünürlük sayıları yukarıdaki ölçüm/uygulama değerleridir; nihai denge, hedef donanım bütçesi veya evrensel standart olarak kesinleştirilmedi.
- **Kapsam:** İç mekânlar, görev/diyalog/kayıt sistemleri ve diğer adalar yapılmış sayılmaz. GDD tasarımı ile uygulanmış ürün birbirinden ayrı tutulur.
- **Kayıt düzeni:** Oturum numarası genel yönergede sabitlenmez. Kullanıcı oturum sınırını açıkça belirlerse esas alınır; mesaj/konu/uygulama açılışı otomatik yeni oturum değildir. Günlük onayla güncellenir; aynı yetkili kapsam için tekrar onay istenmez.

### Önemli dosyalar

- `ArtSource/Environment/BirdIsland/Storky_BirdIsland.blend`
- `UnityProject/Assets/_Game/Art/Environment/BirdIsland/Storky_BirdIsland.fbx` ve `OceanQuad.asset`
- `UnityProject/Assets/_Game/Prefabs/BirdIsland.prefab` ve `StorkPlayer.prefab`
- `UnityProject/Assets/_Game/Scenes/FlightTest.unity`
- `UnityProject/Assets/_Game/Scripts/StorkPrototype.cs`, `InfiniteOcean.cs`, `IslandViewSettings.cs`, `StorkUpdraft.cs`
- `UnityProject/Assets/_Game/Editor/StorkIslandSetup.cs`, `StorkMotionCalibration.cs`
- `UnityProject/Assets/_Game/Animations/Stork.controller`, `Stork_Walk_GroundMatched.anim`
- `UnityProject/Assets/_Game/Shaders/IslandSky.shader`, `StylizedOcean.shader`; ada materyalleri ve `Island_RenderPipeline.asset`
- `UnityProject/ProjectSettings/QualitySettings.asset`, `TagManager.asset`; yeni varlıkların `.meta` dosyaları
- `docs/Storky_GDD.pdf`, `AGENTS.md`, `README.md`, bu çalışma günlüğü

### Kontroller

- **İlk ada kurulumu:** 18 otomatik Play davranış kontrolü geçti: ilerleyen kareler, kararlı başlangıç, klavye/gamepad yürüme/kalkış/roll/dalış, otomatik süzülme, dalışta hız kazanma, stamina tüketimi/yenilenmesi, fare kamerası, duraklatma/devam, denizden dönüş ve hava akımının ilave stamina vermemesi. Bu kayıt, sonraki hareket ayarı öncesindeki durumu ifade eder.
- **Son hareket düzenlemesi:** 13 hedefli kontrol geçti: hız-tempo eşleştirmesi, yere basan ayak kayması, duvarda durma, analog tempo, gövde dönüş merkezi, roll'un rotayı kıvırmaması, açının korunması, gerçek dalıştan destek birikmesi, sıfır stamina ile çıkış desteği, düşüşten toparlanma, kanat çırpmadan yükselişi sürdürme, kanat vuruşunda ileri hız ve yükselme.
- Düz zemin yürüyüş ölçümünde yere basan ayakların ortanca yatay kayma hızı yaklaşık 0,0005 m/s; 112 örnek. Roll ölçümünde gövde merkezinin en büyük farkı yaklaşık 0,006 m, heading sapması 0 dereceydi. Bu sonuçlar yalnız ölçülen koşulları doğrular.
- Sıfır stamina senaryosunda dalıştan çıkış hızı yaklaşık 36,98'den 49,42 m/s'ye çıktı; takip eden 1,6 s'de kanat çırpmadan yaklaşık 49,61 m yükselme gözlendi. Bu değerler test senaryosudur; her dalış için garanti değildir.
- Yakın/yandan yürüyüş ve uzak ada/deniz görüntüleri incelendi. Son ürün kontrolünde güncel Unity Console hata/uyarısı yoktu; geçici giriş cihazları temizlendi, Play kapalı ve sahne kaydedilmiş bırakıldı.
- Belge aşamasındaki dosya denetiminde eksik/orphan `.meta`, Assets içi yinelenen GUID ve bozuk yerel belge bağlantısı bulunmadı. Kaynak `.blend` ve `.fbx` dosyaları yalnız LFS işaretçisi değildi. Belge değişikliği için Unity/Blender yeniden çalıştırılmadı.
- 18 ve 13 kontrol farklı aşamalara aittir; tek bir son tam test paketi veya donanım performansı kanıtı gibi toplanmaz. Fiziksel kontrolcü, tüm arazi, başka bilgisayar ve hedef donanım ölçümü tamamlanmadı.

### Eksikler ve devam noktası

Kullanıcının son hareket hissi geri bildirimi alınacak; özellikle 1,4 m/s yürüyüş ve güçlü dalış çıkışı denge açısından gözden geçirilecek. Eğimli zeminde ayak teması, tüm konma/duvar/kamera rotaları ve gerçek gamepad testi genişletilecek. Diğer adalar ve GDD sistemleri henüz uygulanmadı. Eski leylek Blender kopyası farkı ve tekrar kullanılabilir başlangıç metni açık işlerdir. Gönderim kapsamı ayrıca açık talimat gerektirir.

### Git kaydı

Commit/push yapılmadı. Son görülen mevcut commit `05492b8` (`unity baslangici ve diger degisiklikler`), Oturum 3 değişikliklerinden öncedir. Ada, hareket ve bu belge düzenlemeleri yerel çalışma ağacındadır; uzak depo bu çalışmada sorgulanmadı veya güncellenmedi.

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
