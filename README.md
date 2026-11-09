<div align="center">

# STORKY

### Küçük bir leylek. Büyük bir terfi yarışı.

![Kuş Adası prototipi](https://img.shields.io/badge/Aşama-Kuş%20Adası%20prototipi-f59e0b?style=flat-square)
![Windows PC](https://img.shields.io/badge/Hedef-Windows%20PC-38bdf8?style=flat-square)
![Unity](https://img.shields.io/badge/Unity-6000.3.27f1-a78bfa?style=flat-square)
![Git LFS](https://img.shields.io/badge/Sanat%20varlıkları-Git%20LFS-34d399?style=flat-square)

[🎮 Kurulum](#kurulum) · [🕹️ Kontroller](#kontroller) · [📒 Proje durumu](docs/CALISMA_GUNLUGU.md#guncel-durum) · [🤖 Ajan rehberi](AGENTS.md) · [📄 GDD](docs/Storky_GDD.pdf)

</div>

Storky, bebek üretim ve kargolama şirketinde işe başlayan bir leyleğin terfi yarışını anlatan stilize bir 3D uçuş ve keşif oyunu. Şu an Unity’de Kuş Adası’nın dış mekân greybox’ı ve leyleğin hareket prototipi oynanabilir. Diğer adalar, hikâye ve görevler tasarım aşamasında. Uygulanan özellikler, kararlar ve sıradaki işler [çalışma günlüğünde](docs/CALISMA_GUNLUGU.md#guncel-durum).

<a id="kurulum"></a>

## 🎮 Projeyi aç ve dene

1. Git, Git LFS ve Unity Hub’ı kur. Projenin istediği Unity Editor sürümünü [ProjectVersion.txt](UnityProject/ProjectSettings/ProjectVersion.txt) dosyasından doğrula.
2. Depoyu klonla. Repo kökünde `git lfs install` ve `git lfs pull` çalıştırarak Blender ve FBX dosyalarını indir.
3. Unity Hub’da **Add project from disk** ile `UnityProject/` klasörünü seç ve içe aktarmanın bitmesini bekle.
4. Unity’de [FlightTest.unity](UnityProject/Assets/_Game/Scenes/FlightTest.unity) sahnesini aç ve **Play**’e bas. Leylek fabrika avlusunda başlar.

> [!NOTE]
> Prototipi Editor’da Play ile dene. Kayıtlı build listesinde şu anda Unity şablon sahnesi `SampleScene` etkin. Prototip build’i almak için sahne listesini gözden geçir.

<a id="kontroller"></a>

## 🕹️ Kontroller

| Eylem | Klavye / fare | Xbox / PlayStation |
| --- | --- | --- |
| Yerde yürüme | WASD | Sol çubuk |
| Havada burun aşağı / yukarı | W / S | Sol çubuk ileri / geri |
| Havada sola / sağa dönme | A / D | Sol çubuk sola / sağa |
| Air Roll | Q / E | LB/RB veya L1/R1 |
| Kalkış / kanat çırpma | Her Space basışı | Her A / Çarpı basışı |
| Dalış | Sol Shift basılı | B / Daire basılı |
| Kamera | Fareyi hareket ettir | Sağ çubuk |
| Başlangıca dönme | R | View / Share |
| Duraklatma / devam | Esc | Start / Options |
| Etkileşim için ayrılan giriş | F | X / Kare |

Kanat çırpılmadığında otomatik süzülme devreye girer. Konmak staminayı hızlı, süzülmek yavaş yeniler. Dalışta biriken destek, burnu kaldırırken stamina harcamadan kısa süreli hız ve yükseliş sağlar. Denize düşünce leylek fabrika avlusuna döner.

## 📂 Kaynaklar

| Aradığın şey | Dosya veya klasör |
| --- | --- |
| Güncel durum, kararlar, teknik harita ve oturum geçmişi | [Çalışma günlüğü](docs/CALISMA_GUNLUGU.md) |
| Hedeflenen oyun tasarımı | [Storky GDD](docs/Storky_GDD.pdf) |
| Unity oyunu ve içe aktarılan varlıklar | [UnityProject](UnityProject/) · [`_Game`](UnityProject/Assets/_Game/) |
| Düzenlenebilir Blender kaynakları | [Leylek](ArtSource/Stork/Stork_Hero_Lite.blend) · [Kuş Adası](ArtSource/Environment/BirdIsland/Storky_BirdIsland.blend) |
| Yeni bir modelle çalışmaya başlama | [AGENTS.md](AGENTS.md) |

Yeni bir sohbet veya model için repo linkiyle birlikte **“Ana daldaki AGENTS.md’yi oku ve çalışma günlüğünün üst bölümünden devam et”** demen yeterli. Modelin o çalışma alanında GitHub’a veya klonlanmış depoya erişebilmesi gerekir.
