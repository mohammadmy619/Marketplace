using MarcketDataLayer.DTOs.SellerWallet;
using MarcketDataLayer.Entities.Wallet;
using MarketPlace.DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcketAppliction.Services.Interface
{
  public  interface ISellerWalletService
    {
        #region wallet

        Task<FilterSellerWalletDTO> FilterSellerWallet(FilterSellerWalletDTO filter);
        Task AddWallet(SellerWallet wallet);

        #endregion

    }
}
