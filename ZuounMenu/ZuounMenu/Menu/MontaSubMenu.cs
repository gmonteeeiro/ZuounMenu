﻿using Database.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZuounMenu.Properties;

namespace ZuounMenu.Menu
{
    public class MontaSubMenu
    {
        private readonly Panel pSubMenu;

        /// <summary>
        /// Altura do botão
        /// </summary>
        private readonly int hBtn = 120;

        /// <summary>
        /// Largura do botão
        /// </summary>
        private readonly int wBtn = 100;

        private readonly int marginBtn = 5;

        /// <summary>
        /// Cor do botão
        /// </summary>
        private readonly Color crBtnSt = Color.FromArgb(15, 15, 15);

        public MontaSubMenu(Panel panel)
        {
            pSubMenu = panel;
            InitializePanel();
        }

        public void CarregaMenu(MenuPrincipalDTO pai)
        {
            //Obtém as opções do banco de dados
            SubMenu menu = new SubMenu();
            ArrayList opcoes = menu.GetOpcoes(pai);

            if (opcoes.Count < 1)
            {
                AddBackGroudImage();
                return;
            }

            //Tamanho da tela
            int hTela = pSubMenu.Size.Height;
            int wTela = pSubMenu.Size.Width;

            //Quantidade de itens no menu
            int qtd = opcoes.Count;
            int qtdOpcoesLinha = (wTela / wBtn) - 1;
            int qtdLinhas = qtd / qtdOpcoesLinha;
            if (qtd % qtdOpcoesLinha != 0) qtdLinhas++;

            //Calculo para centralizar os botões
            int iniPosY;
            int iniPosX;

            //Verifica se precisa mais de uma linha 
            if (qtd > qtdOpcoesLinha)
            {
                iniPosY = (hTela / 2) - (qtdLinhas * hBtn / 2);
                iniPosX = (wTela / 2) - (qtdOpcoesLinha * wBtn / 2);
            }
            else
            {
                iniPosY = (hTela / 2) - (hBtn / 2);
                iniPosX = (wTela / 2) - (qtd * wBtn / 2);
            }

            //for para as linhas
            for (int i = 0; i < qtdLinhas; i++)
            {
                int posY = iniPosY + (i * hBtn);

                //Calculas quantos item terão nessa linha
                int restantes = qtd - (qtdOpcoesLinha * i);
                if (restantes > qtdOpcoesLinha) restantes = qtdOpcoesLinha;

                //for para as colunas
                for (int j = 0; j < restantes; j++)
                {

                    int opc = j + i * qtdOpcoesLinha;

                    /* Verifica se precisa recalcular as posições
                     * Necessário para quando tiver poucas opções na linha
                     */
                    int posX;
                    if (restantes < qtdOpcoesLinha)
                    {
                        posX = (wTela / 2) - (restantes * wBtn / 2);
                        posX += j * wBtn;
                    }
                    else
                    {
                        posX = iniPosX + (j * wBtn);
                    }

                    //Pega as informações provinda do banco de dados para mostrar o botão para o usuário
                    SubMenuDTO dto = (SubMenuDTO)opcoes[opc];
                    string nome = dto.Opcao;

                    string btnNome = $"{nome}";

                    AddButton(posY, posX, btnNome, dto.Descricao);
                }
            }
        }

        private void AddBackGroudImage()
        {
            pSubMenu.BackgroundImage = Resources.startup_593327;
            pSubMenu.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void InitializePanel()
        {
            pSubMenu.BackgroundImage = null;
            pSubMenu.Controls.Clear();
        }

        /// <summary>
        /// Adiciona Botão no menu
        /// </summary>
        /// <param name="posY"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        private void AddButton(int posY, int posX, string name, string desc)
        {
            int bSizeW = wBtn - 2 * marginBtn;
            int bSizeH = hBtn - 2 * marginBtn;

            Button b = new Button
            {
                Anchor = (AnchorStyles.Left | AnchorStyles.Top),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(posX, posY),
                Font = new Font("Microsoft Sans Serif", 10F),
                BackColor = crBtnSt,
                ForeColor = Color.White,
                Name = name,
                Size = new Size(bSizeW, bSizeH),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 0, 0, 0),
                TabStop = true,
                Text = desc
            };
            b.FlatAppearance.BorderSize = 0;

            b.Click += new EventHandler(MenuClick);
            pSubMenu.Controls.Add(b);
        }

        /// <summary>
        /// Click de alguma opção do menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            SubMenuOpcoes opc = new SubMenuOpcoes();
            Type t = opc.GetType();
            MethodInfo method = t.GetMethod(b.Name);

            try
            {
                method.Invoke(opc, null);
            }
            catch
            {
                MessageBox.Show("Esse programa ainda está em fase de desenvolvimento, aguarde uma atualização!");
            }
        }

    }
}
