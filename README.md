<div align="center">

# STORKY

### Küçük bir leylek. Şekillenmekte olan bir dünya.

**Low poly · 3D · Unity**

![Aşama: erken geliştirme](https://img.shields.io/badge/AŞAMA-ERKEN_GELİŞTİRME-f59e0b?style=for-the-badge&labelColor=182233)
![Unity 6000.3.27f1](https://img.shields.io/badge/UNITY-6000.3.27f1-38bdf8?style=for-the-badge&labelColor=182233)
![URP 17.3.0](https://img.shields.io/badge/URP-17.3.0-a78bfa?style=for-the-badge&labelColor=182233)
![Git LFS etkin](https://img.shields.io/badge/GIT_LFS-ETKİN-34d399?style=for-the-badge&labelColor=182233)

**[Oyun](#oyun) · [Durum](#durum) · [Kurulum](#kurulum) · [Yol haritası](#yol-haritasi) · [Ekip](#ekip)**

[📒 Çalışma günlüğü ve kararlar](docs/CALISMA_GUNLUGU.md) · [🤖 Ajan yönergeleri](AGENTS.md) · [📄 Oyun tasarımı / GDD](docs/Storky_GDD.pdf)

</div>

> 🟠 **Kuş Adası greybox prototipi** — 600×600 m ada kasabası, deniz ve momentum tabanlı uçuş yerelde hazır. Yürüyüş eşleştirmesi, gövde merkezli Air Roll ve dalıştan toparlanma desteği uygulanıyor; oyun hissi ve hedef donanım testleri sürüyor.

---

<a id="oyun"></a>

## 🪽 Oyun hakkında

Storky, bebek üretim ve kargolama şirketinde işe başlayan bir leyleğin, emekli olacak amirinin yerine geçmek için yarıştığı mizahi bir 3D uçuş ve keşif oyunudur. Tasarım; iki kompakt ana ada, küçük keşif adaları, fabrika görevleri, kasaba etkileşimleri ve mini oyunları kapsar.

Mevcut prototip, Kuş Adası'nın dış mekân greybox'ı ve hareket sistemidir. Hikâye, görevler ve ikinci ana ada henüz oynanabilir olarak kurulmadı.

| Başlık | Durum |
| --- | --- |
| 🎮 Temel oynanış ve oyuncunun amacı | Uçuş, keşif ve görevler; şirketteki terfi yarışında öne çıkma |
| 🖥️ Hedef platformlar | Windows PC ile başlangıç |
| 🧩 Oyun modu | ⚪ Henüz belirlenmedi |
| 🎨 Görsel yaklaşım | Sıcak, stilize low poly 3D; mevcut ada gri ve dokusuz greybox |
| 🛠️ Güncel aşama | Kuş Adası greybox'ı, hareket ayarı ve kullanıcı testi |

<a id="durum"></a>

## 🟢 Proje durumu

- Düzenlenebilir leylek Blender kaynağı.
- Universal Render Pipeline (URP) ile Unity projesi ve Kuş Adası çalışma sahnesi.
- 70 dış bina kütlesi; fabrika, mahalleler, meydan, çarşı, liman ve kırsal kenar.
- Kamerayı takip eden iki üçgenli deniz, tek güneş ve uzak görünüm ayarları.
- Blender ve FBX dosyaları için Git LFS kuralları.
- İşlerin, kararların ve devam noktalarının oturumlar halinde tutulduğu ortak günlük.

**Hazır:** Karakter ve ada FBX aktarımı, Generic rig, altı Animator durumu, kalibre yürüyüş klibi, karakter/ada Prefab'ları, klavye/gamepad kontrolleri ve FlightTest ada sahnesi. **Henüz tamamlanmadı:** Görev, diyalog ve kayıt sistemleri, diğer adalar, nihai hareket dengesi ve hedef donanımda performans doğrulaması.

| Alan | Durum | Açıklama |
| --- | --- | --- |
| Kaynak karakter | 🟢 Hazır | Blender dosyası repoda; önceki model kontrolleri günlükte |
| Unity temeli | 🟢 Kuruldu | URP başlangıç projesi ve SampleScene |
| Unity karakter kurulumu | 🟢 Prototip hazır | FBX, palet materyali, altı Animator durumu, hızla eşleşen yürüyüş ve Prefab |
| Kuş Adası | 🟢 Greybox hazır | 600×600 m, 70 bina, yalnız dış mekân; Blender kaynağı, FBX ve ada Prefab'ı |
| Deniz ve ışık | 🟢 Prototip hazır | Dokusuz mavi deniz, tek güneş, sis ve uzak nesne gizleme |
| Oynanış | 🟠 Test aşaması | Yürüyüş, momentum, stamina, gövde merkezli Air Roll ve dalıştan toparlanma; görev sistemleri bekliyor |
| Yayınlanabilir sürüm | ⚪ Henüz yok | İndirme veya yayın duyurusu bulunmuyor |

## 🧰 Teknoloji

| Araç | Projede kullanılan sürüm / düzen |
| --- | --- |
| Unity Editor | **6000.3.27f1 — Unity 6.3 LTS** |
| URP | 17.3.0; paket sürümlerinin kaynağı `UnityProject/Packages` |
| Blender | 5.2.3 LTS; canlı bağlantı üzerinden doğrulandı |
| Sürüm kontrolü | Git + GitHub; `.blend` ve `.fbx` için Git LFS |

Unity sürümü için esas dosya: [ProjectVersion.txt](UnityProject/ProjectSettings/ProjectVersion.txt). Paketleri ve Editor sürümünü ekipçe karar almadan yükseltmeyin.

<a id="kurulum"></a>

## 🚀 Hızlı başlangıç

1. Git, Git LFS, Unity Hub ve yukarıdaki Unity Editor sürümünü kur.
2. Repoyu GitHub Desktop veya Git ile klonla.
3. Repo klasöründe `git lfs install`, ardından `git lfs pull` çalıştır. Blender dosyası yalnızca birkaç satırlık LFS işaretçisi olarak kalmamalı.
4. Unity Hub'da **Add project from disk** ile **UnityProject** klasörünü seç. Repo kökünü seçme.
5. Paketlerin ve yerel Library klasörünün hazırlanmasını bekle.
6. `Assets/_Game/Scenes/FlightTest.unity` sahnesini aç ve Play ile Kuş Adası prototipini dene. Başlangıç fabrika avlusudur. `Assets/Scenes/SampleScene.unity` şablon olarak korunur.

Blender kaynakları: [Leylek](ArtSource/Stork/Stork_Hero_Lite.blend) ve [Kuş Adası](ArtSource/Environment/BirdIsland/Storky_BirdIsland.blend). Blender'da çalışırken ilgili repo kopyasını aç; başka klasördeki kopyayı düzenlemek bu repoyu güncellemez. Unity için FBX çıktıları `Assets/_Game/Art/` altında tutulur.

### Kontroller

| Eylem | Klavye / fare | Gamepad (Xbox / PlayStation) |
| --- | --- | --- |
| Yerde yürüme | WASD | Sol çubuk |
| Havada burun aşağı / yukarı | W / S | Sol çubuk ileri / geri |
| Havada sağ-sol dönüş | A / D | Sol çubuk sağ / sol |
| Gövde merkezinde Air Roll | Q / E | LB/RB / L1/R1 |
| Kalkış / bir kanat çırpma | Her Space basışı | Her A / Çarpı basışı |
| Dalış | Sol Shift basılı | B / Daire basılı |
| Kamera | Fare hareketi; tuş tutmak gerekmez | Sağ çubuk |
| Başlangıca dönme | R | View / Share |
| Duraklatma / devam | Esc | Start / Options |
| Etkileşim için ayrılmış giriş | F | X / Kare |

Kanat çırpmayınca otomatik süzülme başlar. Stamina kanat çırpmayı sınırlar; konunca hızlı, süzülmede çok yavaş yenilenir. Dalışta biriken destek, dalışı bırakıp burnu kaldırırken stamina gerektirmeden kısa bir toparlanma katkısı verir. Air Roll bırakıldığında açı korunur. Denize düşünce fabrika avlusuna dönülür. Etkileşim girişi ayrıldı; görev/diyalog sistemi henüz kurulmadı.

### Doğrulama sınırları

İlk ada kurulumunda 18 davranış kontrolü; sonraki hareket düzeltmesinde ayak kayması, analog adım temposu, dönüş merkezi ve sıfır stamina ile toparlanma dahil 13 hedefli kontrol geçti. Bu kontroller aynı anda çalıştırılmış nihai bir tam test paketi değildir. Fiziksel kontrolcüyle oyun hissi, eğimli yüzeyler, başka bilgisayarda açılış ve hedef donanımda performans ölçümü bekliyor. Görüş/gölge mesafeleri ve nesne gizleme ayarları başlangıç değerleridir; sabit FPS garantisi değildir.

<details>
<summary><strong>📂 Repo klasörlerini göster</strong></summary>

## Klasör düzeni

```text
Storky-Game/
├── README.md
├── AGENTS.md
├── .gitattributes
├── .gitignore
├── docs/
│   ├── CALISMA_GUNLUGU.md
│   └── Storky_GDD.pdf
├── ArtSource/
│   ├── Stork/Stork_Hero_Lite.blend
│   └── Environment/BirdIsland/Storky_BirdIsland.blend
└── UnityProject/
    ├── .gitignore
    ├── Assets/
    ├── Packages/
    └── ProjectSettings/
```

</details>

<a id="yol-haritasi"></a>

## 🧭 Yol haritası

Bir yayın takvimi değildir; işler kullanıcı kararlarına göre güncellenir.

- [x] Git ve LFS düzenini kur.
- [x] Leylek Blender kaynağını repoya ekle.
- [x] Unity URP başlangıç projesini oluştur.
- [x] README ve oturum günlüğü şablonunu hazırla.
- [x] Ana oyun fikrini ve tasarım kapsamını GDD ile tanımla; ilk hedef platform Windows PC.
- [ ] GDD'deki hikâye, görev, diyalog ve kayıt sistemlerini uygula.
- [x] Unity oyun klasörlerini ve FlightTest çalışma sahnesini düzenle.
- [x] Leyleği Unity'ye aktar; altı klibi ve materyali prototipte doğrula.
- [x] Karakter Prefab'ını ve Animator Controller'ı oluştur.
- [x] Kamera ve temel yürüyüş/uçuş prototipini hazırla.
- [x] Kuş Adası greybox'ını, denizi ve güneşi çalışma sahnesine kur.
- [x] Yürüyüş eşleştirmesi, Air Roll, stamina ve dalıştan toparlanma prototipini hazırla.
- [ ] Kullanıcı geri bildirimiyle hareketi dengele; hedef donanımda performansı ölç.
- [ ] İnsan Adası ve küçük keşif adalarını tasarla ve kur.
- [ ] Projeyi başka ekip bilgisayarında doğrula.

<a id="ekip"></a>

## 🤝 Birlikte çalışma

- Başlarken yerel değişikliklerini kontrol et; ardından uzak değişiklikleri getir ve uygun şekilde güncelle.
- Aynı Blender dosyasında eş zamanlı düzenleme yapmayın. Unity sahneleri ve Prefab'lar için de kimin neyi düzenlediğini konuşun.
- Unity içindeki varlıkları Project penceresinden taşı; `.meta` dosyalarını koru.
- Çalışma sonunda kullanıcı onayıyla ilgili oturum kaydını güncelle. Commit ve push yalnızca açık kullanıcı talimatıyla yapılır.
- **Commit yerel kayıttır; push GitHub'a gönderir.** Otomatik eşitleme varsaymayın.
- Library, Temp ve derleme çıktıları repoya eklenmez. Kaynak `.blend` korunur; `.blend1` ve `.blend2` yedekleri hariç tutulur.

## 📋 Katkı ve kullanım

Görev sahipliği, inceleme akışı ve dağıtım/lisans koşulları henüz belirlenmedi. Repo erişimini, varlıkların yeniden kullanım izni olarak varsaymayın.

---

<div align="center">

**STORKY** · Adım adım geliştiriliyor.

[📒 Oturumları oku](docs/CALISMA_GUNLUGU.md) · [⬆ Başa dön](#storky)

</div>
