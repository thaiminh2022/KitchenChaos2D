shader_type canvas_item;

uniform vec2 offset_speed;

void fragment() {
	vec2 uv =  UV + offset_speed * TIME;
	
	COLOR = texture(TEXTURE, uv);
}


