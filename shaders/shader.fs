#version 330

in vec2 fragTexCoord;
out vec4 finalColor;

uniform sampler2D texture0; // Ana render texture
uniform sampler2D noiseTexture; // Gürültü texture'ı
uniform float time; // Zaman değişkeni
uniform vec2 resolution; // Ekran çözünürlüğü

// Yazı efekti için yeni uniform'lar
uniform vec3 textColor = vec3(0.0, 1.0, 0.0); // Yeşil VHS yazı rengi
uniform float textSpeed = 2.0; // Yazı kayma hızı
uniform float textSize = 0.05; // Yazı boyutu

void main()
{
    // Gürültü efekti
    vec2 uv = fragTexCoord;
    vec4 noise = texture(noiseTexture, uv * 2.0 + vec2(time * 0.3, time * 0.2));
    
    // RGB kanal kaymaları (VHS efekti)
    vec2 offset = vec2(noise.r * 0.02, noise.g * 0.02);
    float r = texture(texture0, uv + offset).r;
    float g = texture(texture0, uv).g;
    float b = texture(texture0, uv - offset).b;
    vec3 color = vec3(r, g, b);
    
    // Yazı efekti
    float textTime = time * textSpeed;
    vec2 textUV = uv * vec2(3.0, 1.0) + vec2(-textTime, 0.0);
    
    // Yazı deseni (örnek: "ERROR 404" tekrarı)
    float textPattern = sin(textUV.x * 100.0) * sin(textUV.y * 20.0 + textTime);
    float textMask = smoothstep(0.3, 0.7, abs(textPattern)) * noise.a;
    
    // Final renk kombinasyonu
    color = mix(color, textColor, textMask * 0.5); // Yazıyı ana renge karıştır
    
    // Film gren efekti (opsiyonel)
    color += noise.rgb * 0.1;
    
    finalColor = vec4(color, 1.0);
}