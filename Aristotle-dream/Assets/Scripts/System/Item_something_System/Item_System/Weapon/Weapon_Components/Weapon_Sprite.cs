using System;
using System.Linq;
using UnityEngine;

namespace Badtime
{
    public class Weapon_Sprite : Weapon_Component<Weapon_Sprite_Component_Data , Attack_Sprites>//该类关注武器的基础渲染器(weapon的附属组件)
    {
        private SpriteRenderer base_sprite_renderer;
        private SpriteRenderer weapon_sprite_renderer;

        private int current_weapon_sprite_index;
        private Sprite[] current_phase_sprites;

        protected override void Handle_Enter()
        {
            base.Handle_Enter();
            current_weapon_sprite_index = 0;
        }

        private void Handle_Enter_Attack_Phase(Attack_Phases phase)
        {
            current_weapon_sprite_index = 0;
            current_phase_sprites = current_attack_data.phase_sprites.FirstOrDefault(data => data.Phase == phase).Sprites;
        }

        private void Handle_Base_Sprite_Change(SpriteRenderer sr)
        {
            if(!is_attack_active)
            {
                weapon_sprite_renderer.sprite = null;
                return;
            }

            //Sprite[] current_attack_sprites = current_attack_data.Sprites;
            
            if(current_weapon_sprite_index >= current_phase_sprites.Length)
            {
                Debug.LogError($"{weapon.name} weapon sprites length mismatch!");
                return;
            }

            weapon_sprite_renderer.sprite = current_phase_sprites[current_weapon_sprite_index];
            current_weapon_sprite_index++;
        }

        protected override void Awake()
        {
            base.Awake();
           
        }

        protected override void Start()
        {
            base.Start();
            base_sprite_renderer = weapon.base_game_object.GetComponent<SpriteRenderer>();
            weapon_sprite_renderer = weapon.weapon_sprite_gameobject.GetComponent<SpriteRenderer>();
            
            data = weapon.Data.Get_Data<Weapon_Sprite_Component_Data>();
            base_sprite_renderer.RegisterSpriteChangeCallback(Handle_Base_Sprite_Change);
            anim_handler.On_Enter_Attack_Phase += Handle_Enter_Attack_Phase;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            base_sprite_renderer.UnregisterSpriteChangeCallback(Handle_Base_Sprite_Change);
            anim_handler.On_Enter_Attack_Phase -= Handle_Enter_Attack_Phase;
        }
    }
}
