<div align="center">

# 📒 Storky · Çalışma günlüğü

[Güncel durum](#guncel-durum) · [Proje kararları](#proje-kararlari) · [Sıradaki işler](#siradaki-isler) · [Dosya haritası](#dosya-haritasi) · [Oturum arşivi](#oturum-arsivi)

</div>

<a id="guncel-durum"></a>

## 🎮 Güncel durum

| Alan | Devralırken bilinmesi gereken |
| --- | --- |
| 🧭 **Son oturum** | **[Oturum 4](#oturum-4)** · Modelleme standardı, 180 FPS hedefi ve karakteri takip eden dayanıklılık göstergesi tamamlandı. Hedef donanım ölçümü bekliyor. |
| 🎯 **Oyun hedefi** | Windows PC’de stilize low poly uçuş ve keşif. Leylek, bebek üretim ve kargolama şirketindeki terfi yarışında ilerler. Dünya ve görev tasarımı [GDD’de](Storky_GDD.pdf). |
| 🏝️ **Oynanabilir sahne** | [FlightTest](../UnityProject/Assets/_Game/Scenes/FlightTest.unity). 600×600 m Kuş Adası, 70 dış bina kütlesi, fabrika avlusu başlangıcı, dokusuz deniz ve tek güneş. |
| 🪽 **Çalışan hareket** | Hızla eşleşen yürüyüş, kalkış ve tek vuruşlu kanat çırpma, otomatik süzülme, stamina, gövde merkezli Air Roll, dalıştan toparlanma ve serbest fare kamerası. Deniz teması fabrika avlusuna döndürür. |
| 🧩 **Tasarımda bekleyenler** | İnsan Adası, küçük keşif adaları, iç mekânlar, hikâye, görev, diyalog ve kayıt sistemleri. Etkileşim girişi ayrılmış durumda. |
| 🧰 **Teknik ortam** | Unity 6000.3.27f1, URP 17.3.0, Input System 1.20.1. [Editor sürümü](../UnityProject/ProjectSettings/ProjectVersion.txt) · [Paket manifesti](../UnityProject/Packages/manifest.json) · [Paket kilidi](../UnityProject/Packages/packages-lock.json) |
| 🧪 **Test durumu** | Son hareket düzenlemesinde 13 hedefli Play kontrolü geçti. Fiziksel kontrolcüyle oyun hissi, eğimli arazide ayak teması, temiz klonla açılış ve hedef donanım ölçümü sırada. Ölçüm koşulları [Oturum 3’te](#oturum-3). |

<a id="proje-kararlari"></a>

## 🧭 Proje kararları

**🟢 Geçerli** · uygulanan proje yönü. **🟡 Deneme** · uygulanan ve geri bildirimle ayarlanabilen değer. **🔵 Plan** · tasarım kapsamı. **⚪ Açık** · karar veya doğrulama bekliyor.

| Konu | Durum | Şu an geçerli karar veya açık nokta | Dayanak |
| --- | --- | --- | --- |
| Hedef platform | 🟢 Geçerli | Önce Windows PC. | [Oturum 2](#oturum-2) |
| Kaynak düzeni | 🟢 Geçerli | Tek repo. Düzenlenebilir Blender kaynakları `ArtSource/`, Unity ve FBX çıktıları `UnityProject/` altında. Ölçek 1 Unity birimi = 1 m. `.blend` ve `.fbx` için Git LFS, Unity varlıkları için eşleşen `.meta` dosyaları. | [Oturum 1](#oturum-1) · [Oturum 2](#oturum-2) |
| Görsel yön ve ada kapsamı | 🟢 Geçerli | Oyun sıcak ve stilize low poly hedefliyor. Karakter ve ada sıfırdan tasarlanabilir. Nihai biçim, oranlar ve yerleşim oyun kamerası ve oynanış ihtiyaçlarına göre seçilir. | [GDD](Storky_GDD.pdf) · [Oturum 3](#oturum-3) |
| Karakter aktarımı | ⚪ Açık | Üretim rig’i ve klip düzeni gerekli pozlar, geçişler ve hareket sistemiyle uyum sınandıktan sonra seçilecek. Mevcut aktarımın teknik ayrıntıları oturum arşivinde. | [Oturum 2](#oturum-2) · [Oturum 3](#oturum-3) |
| Hareket ve giriş | 🟢 Geçerli | Fare kamerası tuş tutmadan çalışır. Air Roll gövde ekseninde döner, rotaya otomatik sapma eklemez ve giriş bırakıldığında açıyı korur. Kanat vuruşu stamina harcar. Dalıştan toplanan destek stamina olmadan kullanılabilir. Hava akımı ilave stamina doldurmaz. | [Kontroller](../README.md#kontroller) · [Oturum 3](#oturum-3) |
| Hareket dengesi | 🟡 Deneme | Yürüyüş 1,4 m/s, azami hava hızı 62 m/s, roll 220°/s. Kanat vuruşunun ileri katkısı 5,5 m/s. Dalış desteği birikime göre en fazla 11 m/s katkı sağlar ve 2 s sürer. | [Oturum 3](#oturum-3) |
| Deniz ve görünürlük | 🟡 Deneme | Unity’de kamerayı takip eden iki üçgenli dokusuz deniz, sis ve tek yönlü güneş. Görüş 3.000 m, küçük detay 330 m, ev üst sınırı 1.500 m, gölge 110 m. Hedef donanımda ölçülecek. | [Oturum 3](#oturum-3) |
| Dünya ve oyun döngüsü | 🔵 Plan | İki ana ada ve küçük keşif adaları, terfi yarışı, görevler, kasaba etkileşimleri ve mini oyunlar. Uygulama sırası kullanıcıyla belirlenecek. | [GDD](Storky_GDD.pdf) |
| Kamera ve performans hedefi | 🟢 Geçerli | Oyuncu leyleği yakın üçüncü şahıs kameradan izler. Ryzen 3 1200 / Radeon RX 560 sınıfı Windows PC’de yoğun oynanışta 180 FPS hedeflenir (kare başına yaklaşık 5,56 ms). Çözünürlük ve kalite ayarı açık. | [GDD, s. 6](Storky_GDD.pdf#page=6) · [Oturum 4](#oturum-4) |
| Modelleme ve optimizasyon | 🟢 Geçerli | Model üretiminde ve YZ’ye verilen tariflerde aşağıdaki [model standardı](#model-standardi) uygulanır. | [Oturum 4](#oturum-4) |
| Dayanıklılık göstergesi | 🟢 Geçerli | Sol üst bilgi paneli kaldırıldı. Leyleğin gövdesini takip eden halka ekrana göre dik kalır ve modelin ekran sınırlarının dışına yerleşir. Kalan kanat vuruşu hakkı sıfırsa kırmızı, birse turuncu, iki ve üzeriyse yeşil. Yetersiz dayanıklılıkla vuruş denenirse kırmızı halka yanıp söner. | [Oturum 4](#oturum-4) |
| Oyun modu ve dağıtım | ⚪ Açık | Oyun modu, ekipte inceleme düzeni, dağıtım ve varlıkların yeniden kullanım koşulları belirlenmeyi bekliyor. | [GDD](Storky_GDD.pdf) · [Oturum 1](#oturum-1) |
| Belgeler ve Git | 🟢 Geçerli | Güncel kararlar burada, oturumlar tarihsiz arşivde tutulur. README renkli GitHub şablonunu kullanır. Kullanıcı GDD’yi elden geçirir ve commit/push işlemlerini yürütür. | [Ajan rehberi](../AGENTS.md) |

Karar değiştiğinde ilgili satırın durumunu, kararını ve dayanağını güncelle. Neden değiştiğini o oturumun kısa kaydına ekle.

<a id="model-standardi"></a>

### Modelleme ve optimizasyon standardı

Her model tarifinde oynanış rolünü, oyuncunun onu hangi sıklıkta ve ne kadar süre gördüğünü, en yakın ve olağan kamera görünümünü, aynı karede beklenen kopya sayısını belirt. Geometriyi önce bu görünümlerde seçilen dış hatlara, hareket sırasında bükülen eklemlere ve oyuncunun temas ettiği yüzeylere ayır.

| Model türü | Korunacak ayrıntı | Oyun kamerasında kabul ölçütü |
| --- | --- | --- |
| Storky | Gaga, boyun, açık ve katlı kanat silüeti. Kanat kökü ve bacaklarda bükülmeyi taşıyan kenar akışı ve kemik ağırlıkları. Kemik ve geometri ihtiyacını hedeflenen hareket açıklığında sınayarak belirle. | Yürüyüş, kalkış, kanat vuruşu, süzülme, dalış ve inişte uç pozları ve geçişleri izle. Kanat kökü hacmini korumalı, süzülme ve dalış arkadan ayırt edilebilmeli, yürüyüşte basan ayak zeminde kaymamalı. |
| Toplanabilir eşya ve etkileşimli nesne | İşlevi anlatan dış hat, renk ve yaklaşma yönünden görülen işaret. | Oyuncu normal yaklaşma hızında nesneyi zamanında tanıyabilmeli. Etkileşim alanı kullanım mesafesine uymalı. |
| Bina, çatı ve yön bulma noktası | Havadan çatı profili ve ana kütle, inişte saçak ve konma yüzeyi. | Yön bulmak için tasarlanan yapılar uçuş rotasından ayırt edilebilmeli. Yakından görünen çatı eğimi ile fiziksel iniş yüzeyi örtüşmeli. |
| Tekrarlanan ağaç, kaya ve dekor | Birkaç ayırt edilebilir silüet, ortak mesh ve materyali kullanan kopyalar. | Yoğun bir uçuş rotasında kopyaları gölgeleriyle birlikte değerlendir. Uzak modelde küçük dal ve çıkıntıları önce sadeleştir. |
| Uzak ada ve arazi | Kıyı çizgisi, sırt profili ve büyük renk alanları. | Yüksekten uçuşta rota okunabilmeli. Yaklaşılabilen arazide ayrıntı geçişi ufuk çizgisinde görünür sıçrama yaratmamalı. |

- **Yüzey ve renk.** Opak, düz renkli varlıklarda uygun ortak palet materyalini kullan. UV’leri renk hücresinin içine, kenarlardan pay bırakarak yerleştir. Sert kenarları biçime göre seç. Organik eklemlerde gölgelemeyi hareket halinde değerlendir. Deniz gibi farklı yüzey davranışları ayrı shader kullanabilir.
- **Görünürlük ve fizik.** Bir yüzeyi kaldırmadan önce alttan uçuşu, yamaç yerleşimini ve gölgesini kontrol et. Çarpışma gereken nesnede oynanışı karşılayan en basit collider’ı kullan. Çatılar ve rampalarda konma yüzeyinin eğimini koru.
- **LOD ve tekrar.** Uzak model veya görünürlük eşiğini nesnenin kapladığı ekran alanına ve tanınması gereken mesafeye göre seç. LOD geçişini normal uçuş hızında kontrol et. Mesh birleştirme ve batching kararını yoğun rotadaki çizim çağrıları, görünürlük ve bellek ölçümüne dayandır.
- **Bütçe ve doğrulama.** Her adayın üçgen, Unity vertex ve materyal yuvası sayılarını kaydet. Animasyonlu modelde kemik sayısı ve vertex başına kemik etkisini de kaydet. Görsel kontrolü hedef oyun kamerasıyla yürüyüş, konma ve yoğun çevre üzerinde uçuş sırasında yap. Kesin sınırları hedef Windows PC’de aynı rota, çözünürlük ve kalite ayarlarında ölçülen CPU/GPU kare süreleri ile bellek kullanımına göre belirle.

#### LOD0 üçgen bütçeleri

Sayılar, tek varlığın en ayrıntılı oyun içi sürümü için Unity’ye aktarılan üçgenleri belirtir.

| Varlık | Önerilen üçgen aralığı |
| --- | ---: |
| Oyuncu leyleği | 3.000–6.000 |
| Yakından görülen NPC veya hayvan | 800–2.500 |
| Taşınan paket veya görev eşyası | 100–600 |
| Tekrarlanan küçük dekor (parça başına) | 12–250 |
| Kaya veya çalı | 30–200 |
| Ağaç | 150–700 |
| Standart ev veya dükkân (tüm dış kabuk) | 300–1.500 |
| Fabrika veya fener gibi özgün yapı (ana dış kütle) | 1.500–5.000 |

İlk oyuncu modelinde yaklaşık **4.000 üçgen ve 1 materyal** hedefle. Daha az üçgenle kabul ölçütlerini karşılayan modeli kullan. Üst sınırın aşılmasını oyun kamerasında görülen silüet veya deformasyon ihtiyacıyla gerekçelendir. Tekrarlanan çevre modellerini aynı karede görünen kopya sayısıyla birlikte değerlendir.

İlk FPS kontrolünü Unity’de FlightTest sahnesini oynatarak Game → Stats üzerinden yap. Fabrika avlusunda ve adanın büyük bölümünü gösteren uçuş açısında FPS ve kare süresini kaydet. 180 FPS hedefini aynı rotada hedef sınıftaki PC’de Windows derlemesiyle doğrula.

<a id="siradaki-isler"></a>

## 🛠️ Sıradaki işler

1. **Üretim temeli.** Yakın üçüncü şahıs kamerayla örnek bir çevrede uçuş ve inişi dene. Leyleğin kanat hareketi ve konacağı yüzey okunuyor mu kontrol et. Hedef sınıftaki PC’de Windows derlemesini aynı yoğun rotada ölç. Çözünürlük, kalite ayarı, ortalama FPS ve en yavaş %1’lik karelerin sürelerini kaydet. Sonuçlarla model ve çevre bütçelerini belirle.
2. **Oyun hissi.** `FlightTest`’te yürüyüş temposunu, Air Roll’u, kanat vuruşunu ve sıfır stamina ile dalıştan çıkışı kullanıcıyla dene. Eğim, çatı inişi ve kamera çarpışmalarını farklı rotalarda kontrol et.
3. **Başka kurulumda doğrulama.** Git LFS ile temiz klondan açılışı, fiziksel gamepad’i ve hedef Windows donanımını sınamayı planla. Bir Windows build’i alınacaksa [build sahne listesini](../UnityProject/ProjectSettings/EditorBuildSettings.asset) kontrol et. Kayıtlı listede `SampleScene` etkin.
4. **Sonraki oyun işi.** Kullanıcı GDD’yi gözden geçirdikten sonra İnsan Adası, keşif adaları ve görev sistemleri arasından sonraki kapsamı belirle.

**Açık kaynak kontrolü.** Leyleğin Blender kaynağını yeniden dışa aktarmadan önce repodaki `.blend` ile Oturum 2’de kullanılan farklı açık kopyanın eşleşmesini doğrula.

<a id="dosya-haritasi"></a>

## 🗺️ Dosya haritası

| İş | Başlangıç noktası |
| --- | --- |
| Sahneyi oynat, başlangıcı veya çevreyi incele | [FlightTest](../UnityProject/Assets/_Game/Scenes/FlightTest.unity) · [BirdIsland Prefab](../UnityProject/Assets/_Game/Prefabs/BirdIsland.prefab) · [StorkPlayer Prefab](../UnityProject/Assets/_Game/Prefabs/StorkPlayer.prefab) |
| Girişler, hareket, kamera, stamina ve dalış | [StorkPrototype.cs](../UnityProject/Assets/_Game/Scripts/StorkPrototype.cs). Prototipin tuş eşlemeleri bu dosyada oluşturuluyor. |
| Yürüyüş, Animator ve kalibrasyon | [Stork.controller](../UnityProject/Assets/_Game/Animations/Stork.controller) · [Yürüyüş klibi](../UnityProject/Assets/_Game/Animations/Stork_Walk_GroundMatched.anim) · [StorkMotionCalibration.cs](../UnityProject/Assets/_Game/Editor/StorkMotionCalibration.cs) |
| Su, sis, gökyüzü, görünürlük ve hava akımı | [InfiniteOcean.cs](../UnityProject/Assets/_Game/Scripts/InfiniteOcean.cs) · [IslandViewSettings.cs](../UnityProject/Assets/_Game/Scripts/IslandViewSettings.cs) · [StorkUpdraft.cs](../UnityProject/Assets/_Game/Scripts/StorkUpdraft.cs) · [Shader’lar](../UnityProject/Assets/_Game/Shaders/) · [Ada URP varlığı](../UnityProject/Assets/_Game/Island_RenderPipeline.asset) |
| Leyleği veya adayı modelle ve içe aktar | [Leylek Blender](../ArtSource/Stork/Stork_Hero_Lite.blend) → [Unity karakter varlıkları](../UnityProject/Assets/_Game/Art/Stork/) · [Ada Blender](../ArtSource/Environment/BirdIsland/Storky_BirdIsland.blend) → [Unity ada varlıkları](../UnityProject/Assets/_Game/Art/Environment/BirdIsland/) |
| Editör üzerinden yeniden kurulum | [StorkIslandSetup.cs](../UnityProject/Assets/_Game/Editor/StorkIslandSetup.cs) ada sahnesini yeniden kurar. [StorkPrototypeSetup.cs](../UnityProject/Assets/_Game/Editor/StorkPrototypeSetup.cs) ilk eğitim alanının kurulumudur. |

> [!IMPORTANT]
> Test için kayıtlı `FlightTest`’i aç ve Play’e bas. `StorkIslandSetup.Build()` sahnedeki Stork Player dışındaki kök nesneleri kaldırır ve sahneyi kaydeder. Hareket kalibrasyonu da klip, Prefab ve sahneyi kaydeder. Bu araçları kullanmadan önce ilgili kodu ve çalışma ağacını incele. Unity Pipeline ve Blender MCP bağlantıları çalışma alanına göre yeniden doğrulanır.

<a id="oturum-arsivi"></a>

## 🗃️ Oturum arşivi

<a id="oturum-1"></a>

### Oturum 1 · Proje ve leylek temeli

<details>
<summary>Kaynak model, Unity projesi ve depo düzeni</summary>

- Ortak depo ve LFS düzeni kuruldu. Unity URP projesi için Force Text ve Visible Meta Files ayarları kontrol edildi. `Library`, `Temp`, `.slnx` ve Blender yedekleri çalışma çıktısı olarak hariç tutuldu.
- Leylek 2.602’den 1.892 üçgene, 1.511’den 1.116 kaynak vertex’e indirildi. Siluet korunarak 19 kemik, tek palet materyali ve altı animasyonla devam edildi. Idle’da nefes ve baş hareketi, Dive’da bacak toplama ve uzatma düzenlendi.
- Önceki Blender çalışmasında 313 karelik koordinat/ağırlık ve dört döngünün uç poz kontrolleri kaydedildi. Bu aşama kaynak modelin kontrolüydü. Windows açılışında görülen Smart App Control bildirimlerinin durduğunu kullanıcı bildirdi. Sorun dönerse engellenen bileşen yeniden incelenmeli.
- Çalışma günlüğü, README ve ajan yönergeleri kuruldu. İlk dört konu tek oturumda toplandı.

</details>

<a id="oturum-2"></a>

### Oturum 2 · Unity’ye aktarım ve ilk uçuş

<details>
<summary>Rig, dışa aktarma ayarları ve ilk oynanış denemesi</summary>

- Unity Pipeline ve Blender bağlantısı kullanıldı. Seçilen dışa aktarma kaynağı açık bir Blender kopyasıydı. Repodaki `.blend` ile eşitliği doğrulanmadı.
- Kaynakta 1.892 üçgen, 1.116 vertex, 19 kemik, 128×128 palet ve vertex başına en çok iki kemik etkisi ölçüldü. Unity aynı mesh için 1.892 üçgen ve 4.633 vertex raporladı.
- 30 FPS NLA aralıkları Idle 1–61, Walk 62–102, Takeoff 103–175, Fly 176–206, Glide 207–267, Dive 268–313. Karakter FBX aktarımında NLA Strips açık, All Actions ve ek leaf bone kapalı, örnekleme 1, sadeleştirme 0, −Z forward ve Y up kullanıldı. Stüdyo ve referans nesneleri dışarıda bırakıldı.
- Generic rig, Root düğümü ve altı Animator durumu kuruldu. Idle, Walk, Fly ve Glide döngülü, Takeoff ve Dive tek seferlik. 300×300 m eğitim alanı ilk `FlightTest` sahnesiydi ve Oturum 3’te yerini adaya bıraktı.
- Sanal girişlerle temel hareket ve animasyon geçişleri gözlendi. Fiziksel XInput cihazı algılandı. PlayStation ve Switch aile metinleri sanal cihazlarla sınandı. XInput olarak tanıtılan cihazın gerçek markası ayırt edilemeyebilir.
- 1080p/60 FPS ve mesh, kemik, doku bütçeleri başlangıç önerileriydi. Kesin bütçe donanım ölçümüne bırakıldı.

</details>

<a id="oturum-3"></a>

### Oturum 3 · Kuş Adası, deniz ve hareket düzeni

<details>
<summary>Ada aktarımı, hareket ayarı ve ölçülen sonuçlar</summary>

- 600×600 m dış mekân adası; fabrika, mahalle, meydan, çarşı, liman, kırsal alan, deniz feneri ve konma yüzeyleriyle kuruldu. Blender sayımında ağaç kopyaları dahil 14.210 üçgen, 152 görünür mesh ve dört ortak gri materyal vardı. Yol-bina kesişmeleri, rıhtım ve iskele rampaları gözden geçirildi. Metre ölçekli üçgenlenmiş FBX’e kamera, ışık, animasyon ve gizli ağaç prototipi alınmadı.
- Kayıtlı `FlightTest` sahnesindeki leylek korundu. Eski eğitim nesnelerinin yerini ada Prefab’ı ve çevre aldı. Ölçekle büyüyen ağaç kapsülleri mesh collider ile düzeltildi. Deniz sisinin yüzey boyunca hesaplanmasıyla yakın su ve ufuk geçişi iyileştirildi. Gölgeler iki kademeli ve 2048 çözünürlükte, 2× MSAA ve SRP Batcher açık, kamera renk/derinlik kopyaları kapalı. Gerçek zamanlı su yansıması ve kırılma prototip kapsamına alınmadı.
- Özgün yürüyüş klibinin ayak hızı yaklaşık 0,35 m/s iken önceki karakter hızı 4 m/s idi. Ayrı kalibre klibin referansı yaklaşık 0,805 m/s. Animator temposu gerçek kat edilen mesafeyi izliyor. Analog girişte azalıyor, duvarda durunca Idle’a dönüyor. Air Roll gövde kemiği çevresine alındı. Dalış desteği kaybedilen irtifadan birikiyor ve kullanımda tüketiliyor.
- **İlk ada kurulumu.** 18 otomatik Play kontrolü geçti. Başlangıç, klavye/gamepad, kamera, duraklatma, denizden dönüş ve hava akımı test edildi. Bu aşama son hareket ayarından önceydi.
- **Hareket düzenlemesi.** 13 hedefli kontrol geçti. Düz zeminde 112 örnekten ortanca yatay ayak kayması yaklaşık 0,0005 m/s. Roll’da en büyük gövde merkezi farkı yaklaşık 0,006 m, rota sapması 0°. Sıfır stamina senaryosunda hız 36,98’den 49,42 m/s’ye çıktı ve sonraki 1,6 s’de kanat vuruşu olmadan yaklaşık 49,61 m yükselme gözlendi. Bu ölçümler belirtilen test koşullarına ait.
- Son ürün kontrolünde sahne kaydedilmişti, Play kapalıydı ve Unity Console’da hata veya uyarı yoktu. Geçici giriş cihazları kaldırıldı. Hedef donanım performansı ve tüm arazi rotaları ölçülmeyi bekliyor.
- Devir teslim belgeleri yeniden düzenlendi. Güncel kararlar üst tabloda, teknik giriş noktaları dosya haritasında, eski sonuç ve gerekçeler arşivde toplanıyor.

</details>

<a id="oturum-4"></a>

### Oturum 4 · Modelleme standardı ve dayanıklılık göstergesi

<details>
<summary>Model bütçeleri, dayanıklılık halkası ve Play kontrolleri</summary>

- Kuş Adası’nda çatılar hem uzaktan yön bulma hem yakından konma işlevi görüyor. Bu yüzden ayrıntı kararı ekran boyutu, görünme sıklığı, oynanış rolü ve eşzamanlı kopya sayısına bağlandı.
- Kullanıcı, karakter ve greybox’ın baştan yapılabileceğini belirtti. İlk taslaktaki karakter ölçüleri, klip sayısı ve ada düzeni üretim standardından çıkarıldı. Hedef donanım ölçümü ve oyun içi kabul kontrolleri bekliyor.
- GDD üçüncü şahıs kamerayı tanımlıyor. Kullanıcı yakın takip görünümünü ve eski PC sınıfında 180 FPS hedefini netleştirdi. Performansın hedef donanımda doğrulanması bekliyor.
- Kullanıcı, varlık türüne göre LOD0 aralıklarını, oyuncu leyleği için yaklaşık 4.000 üçgen ve tek materyal hedefini onayladı. Unity içi ilk FPS ölçümü bekliyor.
- Bilgi panelinin yerini gövdenin sol üstünü takip eden dayanıklılık halkası aldı. Dolgu harcanan dayanıklılığı kademeli gösterir. Kamera çevrildiğinde veya Air Roll sırasında kanatla çakışma olursa halka boş bir kenara geçer. Renk eşikleri kalan vuruş hakkına bağlandı.
- FlightTest Play modunda kanat vuruşunun dayanıklılığı harcadığı, dolgunun kademeli azaldığı ve yetersiz dayanıklılık uyarısının tetiklendiği doğrulandı. Örneklenen 96 kamera açısı ve Air Roll kombinasyonunda halka görünür kaldı ve modelin ekran sınırlarıyla çakışmadı. UI Toolkit teması eklendikten sonra Play kontrolünde Console 0 hata ve 0 uyarı gösterdi.

</details>

<details>
<summary>Sonraki oturum için kayıt şablonu</summary>

```markdown
### Oturum N · Kısa başlık

- Yapılan iş ve sonuç
- Değişen kararın gerekçesi ve ilgili dosyalar
- Yapılan kontroller ve kalan doğrulama
- Sonraki adım
```

</details>
