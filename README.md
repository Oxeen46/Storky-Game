<div align="center">

# STORKY

### Küçük bir leylek. Şekillenmekte olan bir dünya.

**Low poly · 3D · Unity**

![Aşama: erken geliştirme](https://img.shields.io/badge/AŞAMA-ERKEN_GELİŞTİRME-f59e0b?style=for-the-badge&labelColor=182233)
![Unity 6000.3.27f1](https://img.shields.io/badge/UNITY-6000.3.27f1-38bdf8?style=for-the-badge&labelColor=182233)
![URP 17.3.0](https://img.shields.io/badge/URP-17.3.0-a78bfa?style=for-the-badge&labelColor=182233)
![Git LFS etkin](https://img.shields.io/badge/GIT_LFS-ETKİN-34d399?style=for-the-badge&labelColor=182233)

**[Oyun](#oyun) · [Durum](#durum) · [Kurulum](#kurulum) · [Yol haritası](#yol-haritasi) · [Ekip](#ekip)**

[📒 Çalışma günlüğü ve kararlar](docs/CALISMA_GUNLUGU.md) · [🤖 Ajan yönergeleri](AGENTS.md)

</div>

> 🟠 **İlk kontrol prototipi** — Leylek Unity'ye aktarıldı; yürüyüş ve uçuş test alanı yerelde hazır. Kullanıcı testi ve GitHub'a gönderim bekleniyor.

---

<a id="oyun"></a>

## 🪽 Oyun hakkında

Low poly leylek karakteri etrafında geliştirilen bir 3D oyun projesi.

| Başlık | Durum |
| --- | --- |
| 🎮 Temel oynanış ve oyuncunun amacı | ⚪ Henüz belirlenmedi |
| 🖥️ Hedef platformlar | Windows PC ile başlangıç |
| 🧩 Oyun modu | ⚪ Henüz belirlenmedi |
| 🎨 Görsel yaklaşım | Low poly 3D |
| 🛠️ Güncel aşama | Yürüyüş/uçuş prototipi ve kullanıcı testi |

<a id="durum"></a>

## 🟢 Proje durumu

- Düzenlenebilir leylek Blender kaynağı.
- Universal Render Pipeline (URP) ile Unity başlangıç projesi.
- Blender ve FBX dosyaları için Git LFS kuralları.
- İşlerin, kararların ve devam noktalarının oturumlar halinde tutulduğu ortak günlük.

**Hazır:** FBX aktarımı, altı animasyon, Generic rig, materyal, Animator, Prefab ve klavye/gamepad kontrollü FlightTest sahnesi. **Henüz tamamlanmadı:** Nihai oyun döngüsü, kullanıcı testine göre kontrol ayarları ve hedef donanımda performans doğrulaması. Bu bir test prototipidir.

| Alan | Durum | Açıklama |
| --- | --- | --- |
| Kaynak karakter | 🟢 Hazır | Blender dosyası repoda; önceki model kontrolleri günlükte |
| Unity temeli | 🟢 Kuruldu | URP başlangıç projesi ve SampleScene |
| Unity karakter kurulumu | 🟢 Prototip hazır | FBX, palet materyali, altı klip, Generic rig, Animator ve Prefab |
| Oynanış | 🟠 Test aşaması | Yürüyüş, kalkış, uçuş, süzülme, dalış ve takip kamerası; nihai oyun döngüsü açık |
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
6. `Assets/_Game/Scenes/FlightTest.unity` sahnesini aç ve Play ile kontrol prototipini dene. `Assets/Scenes/SampleScene.unity` şablon olarak korunur.

Blender kaynağı: [Stork_Hero_Lite.blend](ArtSource/Stork/Stork_Hero_Lite.blend). Blender'da çalışırken repodaki bu kopyayı aç; başka klasördeki kopyayı düzenlemek bu repoyu güncellemez.

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
│   └── CALISMA_GUNLUGU.md
├── ArtSource/
│   └── Stork/Stork_Hero_Lite.blend
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
- [ ] Oyun fikrini ve oyun döngüsünü netleştir. İlk hedef platform Windows PC olarak seçildi.
- [x] Unity oyun klasörlerini ve FlightTest çalışma sahnesini düzenle.
- [x] Leyleği Unity'ye aktar; altı klibi ve materyali prototipte doğrula.
- [x] Karakter Prefab'ını ve Animator Controller'ı oluştur.
- [x] Kamera ve temel yürüyüş/uçuş prototipini hazırla.
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
